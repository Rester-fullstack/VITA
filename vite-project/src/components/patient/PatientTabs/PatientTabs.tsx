import { useState, type ReactNode } from "react";

import "./PatientTabs.css";

type Tab = {
  key: string;
  label: string;
  icon: string;
  content: ReactNode;
};

type Props = {
  tabs: Tab[];
};

export default function PatientTabs({
  tabs
}: Props){

  const [active, setActive] =
    useState(tabs[0]?.key);

  const activeTab =
    tabs.find(tab =>
      tab.key === active
    );

  return(
    <div className="patient-tabs-card">

      <div className="patient-tabs-header">
        {
          tabs.map(tab => (
            <button
              key={tab.key}
              className={
                active === tab.key
                  ? "tab-btn active"
                  : "tab-btn"
              }
              onClick={() =>
                setActive(tab.key)
              }
            >
              <span>{tab.icon}</span>
              {tab.label}
            </button>
          ))
        }
      </div>

      <div className="patient-tabs-content">
        {activeTab?.content}
      </div>

    </div>
  );
}