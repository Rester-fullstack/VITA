import type { ReactNode } from "react";

import "./StatCard.css";

type StatCardProps = {
  title: string;
  value: number;
  icon: ReactNode;
  color?: string;
};

export default function StatCard({
  title,
  value,
  icon,
  color = "#2563EB"
}: StatCardProps) {
  return (
    <div
      className="stat-card"
      style={{
        borderTop: `4px solid ${color}`
      }}
    >
      <div
        className="stat-icon"
        style={{
          background: `${color}22`,
          color
        }}
      >
        {icon}
      </div>

      <div className="stat-info">
        <span>
          {title}
        </span>

        <h2>
          {value}
        </h2>
      </div>
    </div>
  );
}