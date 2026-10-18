import {DatePeriodPicker} from "../../components/inputs/DatePeriodPicker.tsx";
import {Divider} from "../../components/Divider.tsx";
import {useEffect, useState} from "react";
import {DatePeriod} from "../../data/DatePeriod.ts";
import {DateHelper} from "../../helpers/DateHelper.ts";
import {DateUnit} from "../../data/DateUnit.ts";
import {useEntitiesCache} from "../../cache/context/useEntitiesCache.ts";
import {Loading} from "../../components/Loading.tsx";
import {ValidationError} from "../../components/ValidationError.tsx";
import {StatisticsPerActivityList} from "./StatisticsPerActivityList.tsx";
import {StatisticsPerPeriodRequest} from "../../clients/models/statistics/StatisticsPerPeriodRequest.ts";
import {DatePeriodHelper} from "../../helpers/DatePeriodHelper.ts";
import {StatisticsPerPeriodEntity} from "../../entities/StatisticsPerPeriodEntity.ts";
import {StatisticsPerActivityChart} from "./StatisticsPerActivityChart.tsx";

export function StatisticsDashboard() {
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | undefined>();

  const [unit, setUnit] = useState<DateUnit>(DateUnit.Day);
  const [period, setPeriod] = useState<DatePeriod>(DateHelper.getPeriod(DateUnit.Day, new Date()));
  const [entity, setEntity] = useState<StatisticsPerPeriodEntity | undefined>();

  const cache = useEntitiesCache();

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
        setError(error instanceof Error ? error.message : "Failed to load data");
      }
    }).finally(() => {
      if (current) {
        setLoading(false);
      }
    });

    return () => {
      current = false;
    };
  }, [unit, period, cache.statistics]);

  function changePeriod(unit: DateUnit, value: DatePeriod) {
    setUnit(unit);
    setPeriod(value);
  }

  if (loading) {
    return (<Loading/>)
  }

  return (
      <div style={{display: "flex", flexDirection: "column", flex: "1"}}>
        <DatePeriodPicker onChange={changePeriod}/>
        <Divider/>
        <ValidationError error={error}/>
        {(!entity || entity.perActivities.length === 0) && <span>Nothing</span>}
        {entity && entity.perActivities.length > 0 &&
            <>
                <StatisticsPerActivityChart total={entity.recordedTime} entities={entity.perActivities}/>
                <Divider/>
                <StatisticsPerActivityList entities={entity.perActivities}/>
            </>}
      </div>
  );
}