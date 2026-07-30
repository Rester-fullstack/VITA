import "./PatientHeader.css";

type Props = {

    paciente:any;

    onBack:()=>void;

    onPrint:()=>void;

};

export default function PatientHeader({

    paciente,

    onBack,

    onPrint

}:Props){

    const idade =
        paciente?.dataNascimento
            ? new Date().getFullYear() -
              new Date(
                paciente.dataNascimento
              ).getFullYear()
            : "-";

    return(

        <div className="patient-header-card">

            <div className="patient-left">

                <div className="patient-avatar">

                    {
                        paciente.nome
                            ?.substring(0,1)
                            .toUpperCase()
                    }

                </div>

                <div>

                    <h1>

                        {paciente.nome}

                    </h1>

                    <p>

                        {idade} anos

                    </p>

                </div>

            </div>

            <div className="patient-actions">

                <button

                    className="btn-outline"

                    onClick={onBack}

                >

                    ← Voltar

                </button>

                <button

                    className="btn-primary"

                    onClick={onPrint}

                >

                    📄 Prontuário

                </button>

            </div>

        </div>

    );

}