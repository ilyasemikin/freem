import {StatisticsPerActivityEntity} from "./entities/StatisticsPerActivityEntity.ts";
import {ReactNode, useState} from "react";

export interface IStatisticsPerActivityDonutChartProps {
  activities?: StatisticsPerActivityEntity[];

  size?: number;
  thickness?: number;
  gap?: number;
  grow?: number;
  minAngle?: number;

  children?: ReactNode;
  onSelected?: (entity?: StatisticsPerActivityEntity) => void;
}

class Sector {
  public readonly entity: StatisticsPerActivityEntity;
  public readonly angle: number;
  public readonly color: string;

  public constructor(entity: StatisticsPerActivityEntity, angle: number) {
    this.entity = entity;
    this.angle = angle;
    this.color = "#000000";
  }
}

class StatisticsActivitiesDonutChartProps {
  public readonly entities?: StatisticsPerActivityEntity[];

  public readonly size: number;
  public readonly thickness: number;
  public readonly gap: number;
  public readonly grow: number;
  public readonly minAngle: number;

  public readonly center: number;

  public readonly outerRadius: number;
  public readonly innerRadius: number;

  public readonly children: ReactNode;
  public readonly onSelected?: (entity?: StatisticsPerActivityEntity) => void;

  public constructor(props: IStatisticsPerActivityDonutChartProps) {
    this.entities = props.activities;

    this.size = props.size || 300;
    this.thickness = props.thickness || 30;
    this.gap = props.gap || 2;
    this.grow = props.grow || 3;
    this.minAngle = props.minAngle || 3;

    const radius = this.size / 2;
    this.center = radius + this.grow;

    this.outerRadius = radius;
    this.innerRadius = radius - this.thickness;

    this.children = props.children;
    this.onSelected = props.onSelected;
  }
}

export function StatisticsPerActivityDonutChart(props: IStatisticsPerActivityDonutChartProps) {
  const {
    entities,
    size,
    gap,
    grow,
    minAngle,
    center,
    outerRadius,
    innerRadius,
    children,
    onSelected
  } = new StatisticsActivitiesDonutChartProps(props);

  const [selected, setSelected] = useState<number | null>(null);

  const showed = entities?.filter(e => e.recordedTime.seconds > 0) || [];
  const multiSectors = showed.length > 1;

  const minAngleRad = ((multiSectors ? minAngle : 0) * Math.PI) / 180;

  const totalGap = ((multiSectors ? gap : 0) * showed.length) * (Math.PI / 180);
  const totalSeconds = showed?.reduce((sum, e) => sum + e.recordedTime.seconds, 0);
  const totalMinAnglesRad = minAngleRad * showed.length;
  const usableAngle = 2 * Math.PI - totalGap - totalMinAnglesRad;

  const sectors = showed.map(e => new Sector(
      e,
      multiSectors ? minAngleRad + (e.recordedTime.seconds / totalSeconds) * usableAngle : 2 * Math.PI));

  function select(index: number) {
    const newIndex = selected !== index ? index : null;
    setSelected(newIndex);

    if (onSelected) {
      const entity = newIndex !== null
          ? sectors[newIndex].entity
          : undefined;
      onSelected(entity);
    }
  }

  function polarToCartesian(r: number, angle: number): number[] {
    return [
      center + r * Math.cos(angle),
      center + r * Math.sin(angle),
    ];
  }

  function describeArc(
      startAngle: number,
      endAngle: number,
      innerRadius: number,
      outerRadius: number
  ) {
    let adjustedEndAngle = endAngle;
    if (Math.abs(endAngle - startAngle) >= 2 * Math.PI) {
      adjustedEndAngle = startAngle + 2 * Math.PI - 0.001;
    }

    const [x1, y1] = polarToCartesian(outerRadius, startAngle);
    const [x2, y2] = polarToCartesian(outerRadius, adjustedEndAngle);
    const [x3, y3] = polarToCartesian(innerRadius, adjustedEndAngle);
    const [x4, y4] = polarToCartesian(innerRadius, startAngle);

    const largeArcFlag = adjustedEndAngle - startAngle > Math.PI ? 1 : 0;

    return [
      `M ${x1} ${y1}`,
      `A ${outerRadius} ${outerRadius} 0 ${largeArcFlag} 1 ${x2} ${y2}`,
      `L ${x3} ${y3}`,
      `A ${innerRadius} ${innerRadius} 0 ${largeArcFlag} 0 ${x4} ${y4}`,
      'Z',
    ].join(' ');
  }

  function createSectorPath(startAngle: number, endAngle: number, color: string, index: number) {
    const sectorGrow = selected === index ? grow : 0;
    const data = describeArc(startAngle, endAngle, innerRadius - sectorGrow, outerRadius + sectorGrow);

    return (
        <path
            style={{transition: "all 0.15s ease-in-out", cursor: "pointer"}}
            key={index}
            d={data}
            fill={color}
            onClick={() => select(index)}
        />
    );
  }

  function createPaths(sectors: Sector[]): ReactNode[] {
    const paths: ReactNode[] = [];

    let currentAngle = -Math.PI / 2;
    for (let i = 0; i < sectors.length; i++) {
      const sector = sectors[i];

      const startAngle = currentAngle;
      const endAngle = currentAngle + sector.angle;

      const path = createSectorPath(startAngle, endAngle, sector.color, i);
      paths.push(path);

      currentAngle = endAngle + (multiSectors ? gap : 0) * (Math.PI / 180);
    }

    return paths;
  }

  const paths = createPaths(sectors);

  const svgSize = size + 2 * grow;

  const contentStart = size / 4;
  const contentSize = size / 2;

  return (
      <>
        <svg width={svgSize} height={svgSize}>
          {paths}
          <foreignObject x={contentStart} y={contentStart} width={contentSize} height={contentSize}>
            <div style={{
              display: "flex",
              justifyContent: "center",
              alignItems: "center",
              width: "100%",
              height: "100%",
              textAlign: "center"
            }}>
              {children}
            </div>
          </foreignObject>
        </svg>
      </>
  )
}