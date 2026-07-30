import Sidebar from "./Sidebar";
import Topbar from "./Topbar";

import "./MainLayout.css";

interface Props{
  children: React.ReactNode;
}

export default function MainLayout({
  children
}:Props){
  return(
    <div className="layout">
      <Sidebar />

      <div className="content">
        <Topbar />

        <main>
          {children}
        </main>
      </div>
    </div>
  );
}