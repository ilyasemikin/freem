import {DatePeriodPicker} from "./components/inputs/DatePeriodPicker.tsx";
import {Divider} from "./components/Divider.tsx";
import {useEffect, useState} from "react";
import {DatePeriod} from "./data/DatePeriod.ts";
import {DateHelper} from "./helpers/DateHelper.ts";
import {DateUnit} from "./data/DateUnit.ts";
import {useEntitiesCache} from "./contexts/cache/useEntitiesCache.ts";
import {Loading} from "./components/Loading.tsx";
import {StatisticsPerActivityList} from "./StatisticsPerActivityList.tsx";
import {StatisticsPerPeriodRequest} from "./clients/models/statistics/StatisticsPerPeriodRequest.ts";
import {DatePeriodHelper} from "./helpers/DatePeriodHelper.ts";
import {StatisticsPerPeriodEntity} from "./entities/StatisticsPerPeriodEntity.ts";
import {StatisticsPerActivityChart} from "./StatisticsPerActivityChart.tsx";

export function StatisticsDashboard() {
  const [loading, setLoading] = useState(true);

  const [unit, setUnit] = useState<DateUnit>(DateUnit.Day);
  const [period, setPeriod] = useState<DatePeriod>(DateHelper.getPeriod(DateUnit.Day, new Date()));
  const [entity, setEntity] = useState<StatisticsPerPeriodEntity | undefined>();

  const cache = useEntitiesCache();

  useEffect(() => {
    async function load() {
      const unitPeriod = DatePeriodHelper.toPerPeriodRequest(unit, period);
      const request = new StatisticsPerPeriodRequest(unitPeriod);

      const entity = await cache.statistics.perPeriod(request);
      setEntity(entity);
    }

    setLoading(true);
    setEntity(undefined);
    load();
    setLoading(false);
  }, [period]);

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