import {DatePeriodPicker} from "../../components/inputs/DatePeriodPicker.tsx";
import {Divider} from "../../components/Divider.tsx";
import {useEffect, useState} from "react";
import {DatePeriod} from "../../data/DatePeriod.ts";
import {DateHelper} from "../../helpers/DateHelper.ts";
import {DateUnit} from "../../data/DateUnit.ts";
import {useEntitiesCache} from "../../cache/context/useEntitiesCache.ts";
import {PanelHeader} from "../../components/PanelHeader.tsx";
import {Loading} from "../../components/Loading.tsx";
import {ValidationError} from "../../components/ValidationError.tsx";
import {StatisticsPerActivityList} from "./StatisticsPerActivityList.tsx";
import {StatisticsPerPeriodRequest} from "../../clients/models/statistics/StatisticsPerPeriodRequest.ts";
import {DatePeriodHelper} from "../../helpers/DatePeriodHelper.ts";
import {StatisticsPerPeriodEntity} from "../../entities/StatisticsPerPeriodEntity.ts";
import {StatisticsPerActivityChart} from "./StatisticsPerActivityChart.tsx";
import {TimeZoneHelper} from "../../helpers/TimeZoneHelper.ts";
import {useUserSettings} from "../../settings/context/useUserSettings.ts";

export interface IStatisticsDashboardProps {
  version?: number;
}

export function StatisticsDashboard(props: IStatisticsDashboardProps) {
  const {version} = props;

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | undefined>();

  const {timeZoneOffset} = useUserSettings();

  const [unit, setUnit] = useState<DateUnit>(DateUnit.Day);
  const [period, setPeriod] = useState<DatePeriod>(
      () => DateHelper.getPeriod(DateUnit.Day, TimeZoneHelper.today(timeZoneOffset)));
  const [entity, setEntity] = useState<StatisticsPerPeriodEntity | undefined>();

  const cache = useEntitiesCache();

  // The server computes day bounds from the offset, so a new offset needs a reload
  useEffect(() => {
    let current = true;

    async function load() {
      const unitPeriod = DatePeriodHelper.toPerPeriodRequest(unit, period);
      const request = new StatisticsPerPeriodRequest(unitPeriod);

      const entity = await cache.statistics.perPeriod(request);
      if (current) {
        setEntity(entity);
        setError(undefined);
      }
    }

    load().catch(error => {
      if (current) {
        // Data of the previous period must not stay under the new date
        setEntity(undefined);
        setError(error instanceof Error && error.message !== "" ? error.message : "Failed to load data");
      }
    }).finally(() => {
      if (current) {
        setLoading(false);
      }
    });

    return () => {
      current = false;
    };
  }, [unit, version, period, timeZoneOffset, cache.statistics]);

  function changePeriod(unit: DateUnit, value: DatePeriod) {
    setUnit(unit);
    setPeriod(value);
  }

  if (loading) {
    return (
        <>
          <PanelHeader title="Statistics"/>
          <Loading/>
        </>
    );
  }

  return (
      <div style={{display: "flex", flexDirection: "column", flex: "1"}}>
        <PanelHeader title="Statistics"/>
        <DatePeriodPicker onChange={changePeriod}/>
        <Divider/>
        <ValidationError error={error}/>
        {error === undefined && (!entity || entity.perActivities.length === 0) &&
            <div className="empty">No data for this period</div>}
        {entity && entity.perActivities.length > 0 &&
            <>
                <StatisticsPerActivityChart total={entity.recordedTime} entities={entity.perActivities}/>
                <Divider/>
                <StatisticsPerActivityList entities={entity.perActivities}/>
            </>}
      </div>
  );
}