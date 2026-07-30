import "./DashboardCharts.css";

import {
    ResponsiveContainer,
    BarChart,
    Bar,
    XAxis,
    Tooltip,
    PieChart,
    Pie,
    Cell
} from "recharts";

type Item = {
    nome: string;
    valor: number;
};

type Props = {

    consultasPorMes: Item[];

    documentosEmitidos: Item[];

};

const COLORS = [

    "#2563EB",

    "#10B981",

    "#F59E0B",

    "#EF4444",

    "#8B5CF6"

];

export default function DashboardCharts({

    consultasPorMes,

    documentosEmitidos

}: Props){

    return(

        <div className="charts-grid">

            <div className="chart-card">

                <h2>
                    Consultas por mês
                </h2>

                <ResponsiveContainer
                    width="100%"
                    height={300}
                >

                    <BarChart
                        data={consultasPorMes}
                    >

                        <XAxis
                            dataKey="nome"
                        />

                        <Tooltip/>

                        <Bar
                            dataKey="valor"
                            radius={[8,8,0,0]}
                            fill="#2563EB"
                        />

                    </BarChart>

                </ResponsiveContainer>

            </div>

            <div className="chart-card">

                <h2>
                    Documentos emitidos
                </h2>

                <ResponsiveContainer
                    width="100%"
                    height={300}
                >

                    <PieChart>

                        <Pie

                            data={documentosEmitidos}

                            dataKey="valor"

                            nameKey="nome"

                            outerRadius={100}

                        >

                            {

                                documentosEmitidos.map((_,index)=>(

                                    <Cell

                                        key={index}

                                        fill={COLORS[index%COLORS.length]}

                                    />

                                ))

                            }

                        </Pie>

                        <Tooltip/>

                    </PieChart>

                </ResponsiveContainer>

            </div>

        </div>

    );

}