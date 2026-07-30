import "./PatientInfoCard.css";

type Props = {
    paciente: any;
};

export default function PatientInfoCard({
    paciente
}: Props) {

    return (

        <div className="patient-info-card">

            <div className="patient-info-header">

                <h2>
                    Informações do Paciente
                </h2>

            </div>

            <div className="patient-info-grid">

                <div className="info-item">

                    <span>CPF</span>

                    <strong>

                        {paciente.cpf || "-"}

                    </strong>

                </div>

                <div className="info-item">

                    <span>Telefone</span>

                    <strong>

                        {paciente.telefone || "-"}

                    </strong>

                </div>

                <div className="info-item">

                    <span>Nascimento</span>

                    <strong>

                        {
                            paciente.dataNascimento
                                ? new Date(
                                      paciente.dataNascimento
                                  ).toLocaleDateString("pt-BR")
                                : "-"
                        }

                    </strong>

                </div>

                <div className="info-item">

                    <span>Endereço</span>

                    <strong>

                        {paciente.endereco || "-"}

                    </strong>

                </div>

                <div className="info-item">

                    <span>Email</span>

                    <strong>

                        {paciente.email || "-"}

                    </strong>

                </div>

                <div className="info-item">

                    <span>Convênio</span>

                    <strong>

                        {paciente.convenio || "Não informado"}

                    </strong>

                </div>

                <div className="info-item">

                    <span>Tipo Sanguíneo</span>

                    <strong>

                        {paciente.tipoSanguineo || "--"}

                    </strong>

                </div>

                <div className="info-item">

                    <span>Sexo</span>

                    <strong>

                        {paciente.sexo || "-"}

                    </strong>

                </div>

            </div>

        </div>

    );

}