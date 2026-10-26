import {useMemo, useState} from "react";
import {useNavigate} from "react-router";
import {Button} from "primereact/button";
import {Dropdown, DropdownChangeEvent} from "primereact/dropdown";
import {AppHeader} from "../../components/AppHeader.tsx";
import {DashboardPanel} from "../../components/DashboardPanel.tsx";
import {PanelHeader} from "../../components/PanelHeader.tsx";
import {ValidationError} from "../../components/ValidationError.tsx";
import {TimeZoneOffset} from "../../data/TimeZoneOffset.ts";
import {WeekStart} from "../../data/WeekStart.ts";
import {IUserSettingsChanges} from "../../settings/context/UserSettingsContext.ts";
import {useUserSettings} from "../../settings/context/useUserSettings.ts";

interface ITimeZoneOffsetOption {
  label: string;
  value: number;
}

export function SettingsPage() {
  const {timeZoneOffset, weekStart, updateSettings} = useUserSettings();
  const navigate = useNavigate();

  const [selected, setSelected] = useState<number>(timeZoneOffset.minutes);
  const [selectedWeekStart, setSelectedWeekStart] = useState<WeekStart>(weekStart);
  const [saving, setSaving] = useState(false);
  const [saved, setSaved] = useState(false);
  const [error, setError] = useState<string | undefined>();

  const options = useMemo<ITimeZoneOffsetOption[]>(
      () => TimeZoneOffset.all().map(item => ({label: item.toDisplayString(), value: item.minutes})),
      []);

  const browser = TimeZoneOffset.fromBrowser();

  function select(minutes: number) {
    setSelected(minutes);
    setSaved(false);
    setError(undefined);
  }

  function selectWeekStart(value: WeekStart) {
    setSelectedWeekStart(value);
    setSaved(false);
    setError(undefined);
  }

  function change(e: DropdownChangeEvent) {
    select(e.value);
  }

  const offsetChanged = selected !== timeZoneOffset.minutes;
  const weekStartChanged = !selectedWeekStart.equals(weekStart);
  const changed = offsetChanged || weekStartChanged;

  async function save() {
    setSaving(true);
    const changes: IUserSettingsChanges = {
      timeZoneOffset: offsetChanged ? new TimeZoneOffset(selected) : undefined,
      weekStart: weekStartChanged ? selectedWeekStart : undefined
    };
    const result = await updateSettings(changes);
    setSaving(false);
    setError(result);
    setSaved(result === undefined);
  }

  return (
      <div style={{display: "flex", flexDirection: "column", height: "100%"}}>
        <AppHeader/>
        <div className="dashboard">
          <DashboardPanel style={{maxWidth: "480px", margin: "0 auto"}}>
            <PanelHeader title="Settings"/>
            <div style={{display: "flex", flexDirection: "column", gap: "8px"}}>
              <label htmlFor="time-zone-offset" className="muted" style={{fontSize: "13px"}}>
                Time zone: days, statistics and times of records are shown in it
              </label>
              <div style={{display: "flex", flexDirection: "row", gap: "6px", alignItems: "center"}}>
                <Dropdown
                    inputId="time-zone-offset"
                    style={{flex: 1}}
                    value={selected}
                    options={options}
                    disabled={saving}
                    onChange={change}/>
                <Button
                    text
                    size="small"
                    label={`As in browser (${browser.toDisplayString()})`}
                    disabled={saving}
                    onClick={() => select(browser.minutes)}/>
              </div>
              <span className="muted" style={{fontSize: "13px", marginTop: "8px"}}>
                First day of the week: weeks in statistics, records and calendars start on it
              </span>
              <div className="segmented" style={{alignSelf: "flex-start"}}>
                {WeekStart.all().map(item =>
                    <Button key={item.value} text={!item.equals(selectedWeekStart)}
                            aria-pressed={item.equals(selectedWeekStart)} disabled={saving}
                            onClick={() => selectWeekStart(item)} size="small">{item.label}</Button>)}
              </div>
              <ValidationError error={error}/>
              {saved && <span className="muted" style={{fontSize: "13px"}}>Saved</span>}
              <div style={{display: "flex", flexDirection: "row", gap: "6px", justifyContent: "flex-end"}}>
                <Button text size="small" severity="secondary" label="Back" onClick={() => navigate("/")}/>
                <Button size="small" label="Save" loading={saving} disabled={!changed} onClick={save}/>
              </div>
            </div>
          </DashboardPanel>
        </div>
      </div>
  );
}
