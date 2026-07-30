import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

import api from "../../api/axios";

import "./ConsultaDetalhes.css";

import AgendarRetornoModal from "../../components/modals/AgendarRetornoModal";

import EditConsultaModal from "../../components/modals/EditConsultaModal";

import DeclaracaoComparecimentoModal from "../../components/documents/DeclaracaoComparecimentoModal/DeclaracaoComparecimento";

import { toast } from "react-hot-toast";

import SolicitacaoExameModal from "../../components/modals/SolicitacaoExameModal";

import Timeline from "../../components/patient/Timeline/Timeline";

export default function ConsultaDetalhes() {
  const user =
    JSON.parse(
      localStorage.getItem("user") || "{}"
    );

  const especialidade =
    user.especialidade;

  const { id } = useParams();

  const navigate = useNavigate();

  const [consulta, setConsulta] =
    useState<any>(null);

  const [loading, setLoading] =
    useState(true);

  const [openEditModal, setOpenEditModal] =
    useState(false);

  const [historicos, setHistoricos] =
    useState<any[]>([]);

  const [novaDescricao, setNovaDescricao] =
    useState("");

  const [exames, setExames] =
    useState<any[]>([]);

  const [nomeExame, setNomeExame] =
    useState("");

  const [resultadoExame, setResultadoExame] =
    useState("");

  const [arquivoExame, setArquivoExame] =
    useState<File | null>(null);

  const [receitas, setReceitas] =
    useState<any[]>([]);

  const [medicamento, setMedicamento] =
    useState("");

  const [dosagem, setDosagem] =
    useState("");

  const [frequencia, setFrequencia] =
    useState("");

  const [duracao, setDuracao] =
    useState("");

  const [observacoesReceita, setObservacoesReceita] =
    useState("");

  const [atestados, setAtestados] =
    useState<any[]>([]);

  const [motivoAtestado, setMotivoAtestado] =
    useState("");

  const [cidAtestado, setCidAtestado] =
    useState("");

  const [dataInicioAtestado, setDataInicioAtestado] =
    useState("");

  const [diasAfastamento, setDiasAfastamento] =
    useState("");

  const [observacoesAtestado, setObservacoesAtestado] =
    useState("");

  const [openRetornoModal, setOpenRetornoModal] =
   useState(false);

  const [solicitacoesExames, setSolicitacoesExames] =
   useState<any[]>([]);

  const [
      openSolicitacaoModal,
      setOpenSolicitacaoModal
  ] = useState(false);

  const [declaracoes, setDeclaracoes] =
   useState<any[]>([]);

  const [openDeclaracaoModal, setOpenDeclaracaoModal] =
   useState(false);

  async function loadConsulta() {
    try {
      const response =
        await api.get(`/Consulta/${id}`);

      setConsulta(response.data.data);

    } catch (error) {
      console.error(error);

    } finally {
      setLoading(false);
    }
  }

  async function loadHistoricos() {
    try {
      const response =
        await api.get("/HistoricoClinico");

      const lista =
        response.data.data?.filter(
          (h:any) =>
            Number(h.consultaId) === Number(id)
        ) || [];

      setHistoricos(lista);

    } catch (error) {
      console.error(error);
    }
  }

  async function loadExames() {
    try {
      const response =
        await api.get("/Exame");

      const lista =
        response.data.data?.filter(
          (e:any) =>
            Number(e.consultaId) === Number(id)
        ) || [];

      setExames(lista);

    } catch (error) {
      console.error(error);
    }
  }

  async function loadReceitas() {
    try {
      const response =
        await api.get("/Receita");

      const lista =
        response.data.data?.filter(
          (r:any) =>
            Number(r.consultaId) === Number(id)
        ) || [];

      setReceitas(lista);

    } catch (error) {
      console.error(error);
    }
  }

  async function loadAtestados() {
    try {
      const response =
        await api.get("/Atestado");

      const lista =
        response.data.data?.filter(
          (a:any) =>
            Number(a.consultaId) === Number(id)
        ) || [];

      setAtestados(lista);

    } catch (error) {
      console.error(error);
    }
  }

  async function loadDeclaracoes(){
    try{
      const response =
        await api.get(
          `/DeclaracaoComparecimento/consulta/${id}`
        );

      setDeclaracoes(
        response.data.data ?? []
      );

    }catch(error){
      console.error(error);
    }
  }

  async function loadSolicitacoesExames(){
    try{
      const response =
        await api.get(
          `/SolicitacaoExame/consulta/${id}`
        );

      setSolicitacoesExames(
        response.data.data ?? []
      );

    }catch(error){
      console.error(error);
    }
  }

  async function salvarHistorico() {
    if (!novaDescricao.trim()) return;

    try {
      await api.post(
        "/HistoricoClinico",
        {
          descricao: novaDescricao,
          consultaId: consulta.id,
          pacienteId: consulta.pacienteId
        }
      );

      setNovaDescricao("");

      await loadHistoricos();

    } catch (error) {
      console.error(error);
    }
  }

  async function salvarExame() {
    if (
      !nomeExame.trim() ||
      !resultadoExame.trim()
    ) return;

    try {
      const formData =
        new FormData();

      formData.append("nome", nomeExame);
      formData.append("resultado", resultadoExame);
      formData.append("consultaId", consulta.id);
      formData.append("pacienteId", consulta.pacienteId);

      await api.post(
        "/Exame",
        formData
      );

      setNomeExame("");
      setResultadoExame("");

      await loadExames();

    } catch (error) {
      console.error(error);
    }
  }

  async function uploadPdf(exameId:number) {
    if (!arquivoExame) {
      alert("Selecione um PDF primeiro");
      return;
    }

    try {
      const formData =
        new FormData();

      formData.append("file", arquivoExame);

      await api.post(
        `/Exame/upload/${exameId}`,
        formData
      );

      alert("PDF enviado com sucesso");

      setArquivoExame(null);

      await loadExames();

    } catch (error) {
      console.error(error);
      alert("Erro ao enviar PDF");
    }
  }

  async function salvarReceita() {
    if (
      !medicamento.trim() ||
      !dosagem.trim() ||
      !frequencia.trim() ||
      !duracao.trim()
    ) return;

    try {
      await api.post(
        "/Receita",
        {
          medicamento,
          dosagem,
          frequencia,
          duracao,
          observacoes:
            observacoesReceita,
          consultaId:
            consulta.id,
          pacienteId:
            consulta.pacienteId
        }
      );

      setMedicamento("");
      setDosagem("");
      setFrequencia("");
      setDuracao("");
      setObservacoesReceita("");

      await loadReceitas();

    } catch (error) {
      console.error(error);
    }
  }

  async function salvarAtestado() {
    if (
      !motivoAtestado.trim() ||
      !dataInicioAtestado ||
      !diasAfastamento
    ) return;

    try {
      await api.post(
        "/Atestado",
        {
          motivo: motivoAtestado,
          cid: cidAtestado,
          dataInicio: dataInicioAtestado,
          diasAfastamento: Number(diasAfastamento),
          observacoes: observacoesAtestado,
          consultaId: consulta.id,
          pacienteId: consulta.pacienteId
        }
      );

      setMotivoAtestado("");
      setCidAtestado("");
      setDataInicioAtestado("");
      setDiasAfastamento("");
      setObservacoesAtestado("");

      await loadAtestados();

    } catch (error) {
      console.error(error);
    }
  }

  async function alterarStatus(
    novoStatus:string
  ) {
    try {
      await api.put(
        `/Consulta/${consulta.id}`,
        {
          pacienteId: consulta.pacienteId,
          medicoId: consulta.medicoId,
          dataConsulta: consulta.dataConsulta,
          observacoes: consulta.observacoes,
          status: novoStatus
        }
      );

      await loadConsulta();

    } catch (error) {
      console.error(error);
    }
  }

  async function abrirPdfAtestado(
    atestadoId:number
  ) {
    try {
      const response =
        await api.get(
          `/Atestado/pdf/${atestadoId}`,
          {
            responseType: "blob"
          }
        );

      const fileURL =
        URL.createObjectURL(
          new Blob(
            [response.data],
            {
              type: "application/pdf"
            }
          )
        );

      window.open(fileURL, "_blank");

    } catch (error) {
      console.error(error);
      alert("Erro ao abrir PDF do atestado");
    }
  }

  async function abrirPdfSolicitacao(id:number){
    try{
      const response =
        await api.get(
          `/SolicitacaoExame/pdf/${id}`,
          {
            responseType:"blob"
          }
        );

      const fileURL =
        URL.createObjectURL(
          new Blob(
            [response.data],
            { type:"application/pdf" }
          )
        );

      window.open(fileURL, "_blank");

    }catch(error){
      console.error(error);
      alert("Erro ao abrir PDF da solicitação");
    }
  }

  async function excluirSolicitacao(id:number){
    const confirmar =
      window.confirm(
        "Deseja excluir esta solicitação?"
      );

    if(!confirmar) return;

    try{
      await api.delete(
        `/SolicitacaoExame/${id}`
      );

      await loadSolicitacoesExames();

    }catch(error){
      console.error(error);
      alert("Erro ao excluir solicitação");
    }
  }

  async function abrirPdfReceita(
    receitaId:number
  ) {
    try {
      const response =
        await api.get(
          `/Receita/pdf/${receitaId}`,
          {
            responseType: "blob"
          }
        );

      const fileURL =
        URL.createObjectURL(
          new Blob(
            [response.data],
            {
              type: "application/pdf"
            }
          )
        );

      window.open(fileURL, "_blank");

    } catch (error) {
      console.error(error);
      alert("Erro ao abrir PDF da receita");
    }
  }

  async function abrirPdfDeclaracao(id:number){
    try{
      const response =
        await api.get(
          `/DeclaracaoComparecimento/pdf/${id}`,
          {
            responseType:"blob"
          }
        );

      const fileURL =
        URL.createObjectURL(
          new Blob(
            [response.data],
            { type:"application/pdf" }
          )
        );

      window.open(fileURL, "_blank");

    }catch(error){
      console.error(error);
      alert("Erro ao abrir PDF da declaração");
    }
  }

  async function excluirReceita(
    receitaId:number
  ){
    const confirmar =
      window.confirm(
        "Deseja excluir esta receita?"
      );

    if(!confirmar)
      return;

    try{

      await api.delete(
        `/Receita/${receitaId}`
      );

      await loadReceitas();

    }catch(error){

      console.error(error);

      alert(
        "Erro ao excluir receita"
      );
    }
  }

  async function excluirConsulta(){
    const confirmar =
      window.confirm(
        "Deseja excluir esta consulta? Esta ação não poderá ser desfeita."
      );

    if(!confirmar) return;

    try{
      await api.delete(
        `/Consulta/${consulta.id}`
      );

      toast.success(
        "Consulta excluída com sucesso"
      );

      navigate("/minhas-consultas");

    }catch(error){
      console.error(error);

      toast.error(
        "Erro ao excluir consulta"
      );
    }
  }

  async function excluirAtestado(
    atestadoId:number
  ){
    const confirmar =
      window.confirm(
        "Deseja excluir este atestado?"
      );

    if(!confirmar)
      return;

    try{

      await api.delete(
        `/Atestado/${atestadoId}`
      );

      await loadAtestados();

    }catch(error){

      console.error(error);

      alert(
        "Erro ao excluir atestado"
      );
    }
  }

  async function excluirDeclaracao(id:number){
    const confirmar =
      window.confirm(
        "Deseja excluir esta declaração?"
      );

    if(!confirmar) return;

    try{
      await api.delete(
        `/DeclaracaoComparecimento/${id}`
      );

      await loadDeclaracoes();

    }catch(error){
      console.error(error);
      alert("Erro ao excluir declaração");
    }
  }

  useEffect(() => {
    if (id) {
      loadConsulta();
      loadHistoricos();
      loadExames();
      loadReceitas();
      loadAtestados();
      loadSolicitacoesExames();
      loadDeclaracoes();
    }
  }, [id]);

  if (loading) {
    return (
      <div className="loading">
        Carregando consulta...
      </div>
    );
  }

  if (!consulta) {
    return (
      <div className="not-found">
        Consulta não encontrada
      </div>
    );
  }

  const dataFormatada =
    new Date(
      consulta.dataConsulta
    ).toLocaleDateString("pt-BR");

  const horaFormatada =
    new Date(
      consulta.dataConsulta
    ).toLocaleTimeString(
      "pt-BR",
      {
        hour: "2-digit",
        minute: "2-digit"
      }
    );


  const timeline = [
    ...historicos.map(h => ({
      id: `historico-${h.id}`,
      tipo: "Evolução",
      icone: "🩺",
      data: h.dataRegistro,
      titulo: "Evolução clínica",
      descricao: h.descricao
    })),

    ...exames.map(e => ({
      id: `exame-${e.id}`,
      tipo: "Exame",
      icone: "🧪",
      data: e.dataExame,
      titulo: e.nome,
      descricao: e.resultado
    })),

    ...receitas.map(r => ({
      id: `receita-${r.id}`,
      tipo: "Receita",
      icone: "💊",
      data: r.dataReceita,
      titulo: `Receita #${r.id}`,
      descricao: `${r.medicamento} • ${r.dosagem} • ${r.frequencia} • ${r.duracao}`
    })),

    ...atestados.map(a => ({
      id: `atestado-${a.id}`,
      tipo: "Atestado",
      icone: "📋",
      data: a.dataEmissao,
      titulo: `Atestado #${a.id}`,
      descricao: `${a.motivo} • ${a.diasAfastamento} dia(s) de afastamento`
    }))
  ].sort(
    (a, b) =>
      new Date(b.data).getTime() -
      new Date(a.data).getTime()
  );

  const statusClasse =
    consulta.status?.toLowerCase();

  return (
    <>
      <div className="consulta-detalhes">

        <div className="consulta-header">
          <div>
            <h1 className="consulta-title">
              Consulta #{consulta.id}
            </h1>

            <p className="consulta-subtitle">
              Informações completas da consulta
            </p>
          </div>

          <div className={`status-badge ${statusClasse}`}>
            {consulta.status}
          </div>
        </div>

        <div className="patient-card">
          <div className="patient-avatar">
            👤
          </div>

          <div>
            <h2>
              {consulta.pacienteNome}
            </h2>

            <span>
              Paciente vinculado
            </span>
          </div>
        </div>

        <div className="consulta-info-grid">

          <div className="info-card">
            <span>👨‍⚕️ Médico</span>
            <strong>{consulta.medicoNome}</strong>
          </div>

          <div className="info-card">
            <span>📅 Data</span>
            <strong>{dataFormatada}</strong>
          </div>

          <div className="info-card">
            <span>🕒 Horário</span>
            <strong>{horaFormatada}</strong>
          </div>

          <div className="info-card">
            <span>🆔 Consulta</span>
            <strong>#{consulta.id}</strong>
          </div>

        </div>

        <div className="observacoes-card">
          <h3>
            📝 Observações
          </h3>

          <p>
            {
              consulta.observacoes ||
              "Nenhuma observação cadastrada."
            }
          </p>
        </div>

        <div className="timeline-card">

          <h3>
            📌 Linha do Tempo Clínica
          </h3>

          {
            timeline.length === 0 ? (

              <p className="timeline-empty">
                Nenhum evento clínico registrado ainda.
              </p>

            ) : (

              <div className="timeline-list">

                {
                  timeline.map(item => (

                    <div
                      key={item.id}
                      className="timeline-item"
                    >

                      <div className="timeline-icon">
                        {item.icone}
                      </div>

                      <div className="timeline-content">

                        <div className="timeline-top">

                          <strong>
                            {item.titulo}
                          </strong>

                          <span>
                            {
                              new Date(
                                item.data
                              ).toLocaleString("pt-BR")
                            }
                          </span>

                        </div>

                        <small>
                          {item.tipo}
                        </small>

                        <p>
                          {item.descricao}
                        </p>

                      </div>

                    </div>

                  ))
                }

              </div>

            )
          }

        </div>

        <div className="prontuario-card">
          <h3>
            🩺 Prontuário Médico
          </h3>

          <textarea
            className="prontuario-input"
            placeholder="Registrar evolução clínica..."
            value={novaDescricao}
            onChange={(e) =>
              setNovaDescricao(e.target.value)
            }
          />

          <button
            className="btn-primary"
            onClick={salvarHistorico}
          >
            Salvar Evolução
          </button>

          <div className="historico-list">
            {
              historicos.map(historico => (
                <div
                  key={historico.id}
                  className="historico-item"
                >
                  <span>
                    {
                      new Date(
                        historico.dataRegistro
                      ).toLocaleString("pt-BR")
                    }
                  </span>

                  <p>
                    {historico.descricao}
                  </p>
                </div>
              ))
            }
          </div>
        </div>

        <div className="exames-card">
          <h3>
            🧪 Exames
          </h3>

          <input
            type="text"
            placeholder="Nome do exame"
            value={nomeExame}
            onChange={(e) =>
              setNomeExame(e.target.value)
            }
          />

          <textarea
            placeholder="Resultado do exame"
            value={resultadoExame}
            onChange={(e) =>
              setResultadoExame(e.target.value)
            }
          />

          <button
            className="btn-primary"
            onClick={salvarExame}
          >
            Salvar Exame
          </button>

          <div className="exames-list">
            {
              exames.map(exame => (
                <div
                  key={exame.id}
                  className="exame-item"
                >
                  <h4>{exame.nome}</h4>

                  <p>{exame.resultado}</p>

                  <input
                    type="file"
                    accept=".pdf"
                    onChange={(e) =>
                      setArquivoExame(
                        e.target.files?.[0] || null
                      )
                    }
                  />

                  {
                    arquivoExame && (
                      <span>
                        📎 {arquivoExame.name}
                      </span>
                    )
                  }

                  <button
                    className="btn-primary"
                    onClick={() =>
                      uploadPdf(exame.id)
                    }
                  >
                    Enviar PDF
                  </button>

                  {
                    exame.pdfUrl ? (
                      <a
                        href={encodeURI(
                          `http://localhost:5182${exame.pdfUrl}`
                        )}
                        target="_blank"
                        rel="noreferrer"
                        className="pdf-link"
                      >
                        📄 Ver PDF
                      </a>
                    ) : (
                      <span>
                        Nenhum PDF anexado
                      </span>
                    )
                  }
                </div>
              ))
            }
          </div>


          <div className="solicitacoes-section">

              <div className="section-header-inline">
                <div>
                  <h3>
                    📋 Solicitações de Exames
                  </h3>

                  <p>
                    Pedidos emitidos para o paciente realizar exames.
                  </p>
                </div>

                <button
                  className="btn-primary"
                  onClick={() =>
                    setOpenSolicitacaoModal(true)
                  }
                >
                  + Nova Solicitação
                </button>
              </div>

              {
                solicitacoesExames.length === 0 ? (

                  <p className="empty-state">
                    Nenhuma solicitação emitida.
                  </p>

                ) : (

                  <div className="solicitacoes-list">
                    {
                      solicitacoesExames.map(item => (

                        <div
                          key={item.id}
                          className="solicitacao-item"
                        >

                          <div className="document-header">
                            <strong>
                              Solicitação #{item.id}
                            </strong>

                            <span className="document-badge exame">
                              Solicitação
                            </span>
                          </div>

                          <span>
                            {
                              new Date(
                                item.dataSolicitacao
                              ).toLocaleString("pt-BR")
                            }
                          </span>

                          <p>
                            {
                              item.examesSolicitados
                                ?.split("\n")
                                .slice(0, 4)
                                .join(", ")
                            }
                          </p>

                          {
                            item.justificativa && (
                              <small>
                                {item.justificativa}
                              </small>
                            )
                          }

                          <div className="document-actions">

                            <button
                              className="btn-pdf"
                              onClick={() =>
                                abrirPdfSolicitacao(item.id)
                              }
                            >
                              📄 PDF
                            </button>

                            <button
                              className="btn-delete"
                              onClick={() =>
                                excluirSolicitacao(item.id)
                              }
                            >
                              🗑 Excluir
                            </button>

                          </div>

                        </div>

                      ))
                    }
                  </div>
                )
              }
          </div>
        </div>

        <div className="receitas-card">
          <h3>
            💊 Receitas Médicas
          </h3>

          <input
            type="text"
            placeholder="Medicamento"
            value={medicamento}
            onChange={(e) =>
              setMedicamento(e.target.value)
            }
          />

          <input
            type="text"
            placeholder="Dosagem. Ex: 500mg"
            value={dosagem}
            onChange={(e) =>
              setDosagem(e.target.value)
            }
          />

          <input
            type="text"
            placeholder="Frequência. Ex: 8 em 8 horas"
            value={frequencia}
            onChange={(e) =>
              setFrequencia(e.target.value)
            }
          />

          <input
            type="text"
            placeholder="Duração. Ex: 3 dias"
            value={duracao}
            onChange={(e) =>
              setDuracao(e.target.value)
            }
          />

          <textarea
            placeholder="Observações da receita..."
            value={observacoesReceita}
            onChange={(e) =>
              setObservacoesReceita(e.target.value)
            }
          />

          <button
            className="btn-primary"
            onClick={salvarReceita}
          >
            Emitir Receita
          </button>

          <div className="receitas-list">
            {
              receitas.map(receita => (
                <div
                  key={receita.id}
                  className="receita-item"
                >
                  <div className="document-header">
                    <strong>
                      Receita #{receita.id}
                    </strong>

                    <span className="document-badge receita">
                      Receita
                    </span>
                  </div>

                  <span>
                    {
                      new Date(
                        receita.dataReceita
                      ).toLocaleString("pt-BR")
                    }
                  </span>

                  <div className="receita-info">
                    <p>
                      <strong>Medicamento:</strong>{" "}
                      {receita.medicamento}
                    </p>

                    <p>
                      <strong>Dosagem:</strong>{" "}
                      {receita.dosagem}
                    </p>

                    <p>
                      <strong>Frequência:</strong>{" "}
                      {receita.frequencia}
                    </p>

                    <p>
                      <strong>Duração:</strong>{" "}
                      {receita.duracao}
                    </p>
                  </div>

                  {
                    receita.observacoes && (
                      <small>
                        {receita.observacoes}
                      </small>
                    )
                  }

                  <div className="document-actions">

                    <button
                      className="btn-pdf"
                      onClick={() =>
                        abrirPdfReceita(receita.id)
                      }
                    >
                      📄 PDF
                    </button>

                    <button
                      className="btn-delete"
                      onClick={() =>
                        excluirReceita(receita.id)
                      }
                    >
                      🗑 Excluir
                    </button>

                  </div>
                </div>
              ))
            }
          </div>
        </div>

        <div className="atestados-card">
          <h3>
            📋 Atestados Médicos
          </h3>

          <input
            type="text"
            placeholder="Motivo do afastamento"
            value={motivoAtestado}
            onChange={(e) =>
              setMotivoAtestado(e.target.value)
            }
          />

          <input
            type="text"
            placeholder="CID (opcional)"
            value={cidAtestado}
            onChange={(e) =>
              setCidAtestado(e.target.value)
            }
          />

          <input
            type="date"
            value={dataInicioAtestado}
            onChange={(e) =>
              setDataInicioAtestado(e.target.value)
            }
          />

          <input
            type="number"
            placeholder="Dias de afastamento"
            value={diasAfastamento}
            onChange={(e) =>
              setDiasAfastamento(e.target.value)
            }
          />

          <textarea
            placeholder="Observações do atestado..."
            value={observacoesAtestado}
            onChange={(e) =>
              setObservacoesAtestado(e.target.value)
            }
          />

          <button
            className="btn-primary"
            onClick={salvarAtestado}
          >
            Emitir Atestado
          </button>

          <div className="atestados-list">
            {
              atestados.map(atestado => (
                <div
                  key={atestado.id}
                  className="atestado-item"
                >
                 <div className="document-header">
                    <strong>
                      Atestado #{atestado.id}
                    </strong>

                    <span className="document-badge atestado">
                      Atestado
                    </span>
                  </div>

                  <span>
                    {
                      new Date(
                        atestado.dataEmissao
                      ).toLocaleString("pt-BR")
                    }
                  </span>

                  <p>
                    <strong>Motivo:</strong>{" "}
                    {atestado.motivo}
                  </p>

                  <p>
                    <strong>CID:</strong>{" "}
                    {atestado.cid || "Não informado"}
                  </p>

                  <small>
                    Início: {
                      new Date(
                        atestado.dataInicio
                      ).toLocaleDateString("pt-BR")
                    } • {atestado.diasAfastamento} dia(s)
                  </small>

                  {
                    atestado.observacoes && (
                      <small>
                        {atestado.observacoes}
                      </small>
                    )
                  }

                  <div className="document-actions">

                    <button
                      className="btn-pdf"
                      onClick={() =>
                        abrirPdfAtestado(atestado.id)
                      }
                    >
                      📄 PDF
                    </button>

                    <button
                      className="btn-delete"
                      onClick={() =>
                        excluirAtestado(atestado.id)
                      }
                    >
                      🗑 Excluir
                    </button>


                  </div>
                </div>
              ))
            }
          </div>
        </div>

        <div className="declaracoes-card">
          <h3>
            📄 Declarações de Comparecimento
          </h3>

          <button
            className="btn-primary"
            onClick={() =>
              setOpenDeclaracaoModal(true)
            }
          >
            + Emitir Declaração
          </button>

          <div className="declaracoes-list">
            {
              declaracoes.length === 0 ? (
                <p className="empty-state">
                  Nenhuma declaração emitida.
                </p>
              ) : (
                declaracoes.map(declaracao => (
                  <div
                    key={declaracao.id}
                    className="declaracao-item"
                  >
                    <div className="document-header">
                      <strong>
                        Declaração #{declaracao.id}
                      </strong>

                      <span className="document-badge atestado">
                        Declaração
                      </span>
                    </div>

                    <span>
                      {
                        new Date(
                          declaracao.dataEmissao
                        ).toLocaleString("pt-BR")
                      }
                    </span>

                    {
                      declaracao.observacoes && (
                        <small>
                          {declaracao.observacoes}
                        </small>
                      )
                    }

                    <div className="document-actions">
                      <button
                        className="btn-pdf"
                        onClick={() =>
                          abrirPdfDeclaracao(declaracao.id)
                        }
                      >
                        📄 PDF
                      </button>

                      <button
                        className="btn-delete"
                        onClick={() =>
                          excluirDeclaracao(declaracao.id)
                        }
                      >
                        🗑 Excluir
                      </button>
                    </div>
                  </div>
                ))
              )
            }
          </div>


          <Timeline
              pacienteId={consulta.pacienteId}
          />



        </div>

        <div className="actions">
          <button
            className="btn-primary"
            onClick={() =>
              navigate(`/paciente/${consulta.pacienteId}`)
            }
          >
            Ver Prontuário
          </button>

          <button
            className="btn-primary"
            onClick={() =>
              setOpenEditModal(true)
            }
          >
            Editar Consulta
          </button>

          <button
            className="btn-primary"
            onClick={() =>
              setOpenRetornoModal(true)
            }
          >
            Agendar Retorno
          </button>

          {especialidade === "Odontologia" && (
            <button
              className="btn-primary"
              onClick={() =>
                navigate(`/consulta/${consulta.id}/odontologia`)
              }
            >
              🦷 Odontograma
            </button>
          )}

          {especialidade === "Psicologia" && (
            <button
              className="btn-primary"
              onClick={() =>
                navigate(`/consulta/${consulta.id}/psicologia`)
              }
            >
              🧠 Psicologia
            </button>
          )}

          {especialidade === "Nutrição" && (
            <button
              className="btn-primary"
              onClick={() =>
                navigate(`/consulta/${consulta.id}/nutricao`)
              }
            >
              🥗 Nutrição
            </button>
          )}

          <button
              className="btn-primary"
              onClick={() =>
                  setOpenSolicitacaoModal(true)
              }
          >
              🧪 Solicitar Exames
          </button>

          <button
            className="btn-primary"
            onClick={() =>
              setOpenDeclaracaoModal(true)
            }
          >
            📄 Emitir Declaração
          </button>

          {
            consulta.status !== "Finalizada" && (
              <button
                className="btn-success"
                onClick={() =>
                  alterarStatus("Finalizada")
                }
              >
                Finalizar Consulta
              </button>
            )
          }

           {
            consulta.status !== "Cancelada" && (
              <button
                className="btn-danger"
                onClick={() =>
                  alterarStatus("Cancelada")
                }
              >
                Cancelar Consulta
              </button>
            )
          }

          {consulta.status === "Agendada" && (
            <button
              className="btn-danger"
              onClick={excluirConsulta}
            >
              Excluir Consulta
            </button>
          )}

          <button
            className="btn-secondary"
            onClick={() =>
              navigate(-1)
            }
          >
            Voltar
          </button>
        </div>

      </div>

      <AgendarRetornoModal
        open={openRetornoModal}
        onClose={() =>
          setOpenRetornoModal(false)
        }
        onSuccess={loadConsulta}
        consulta={consulta}
      />

      <EditConsultaModal
        open={openEditModal}
        onClose={() =>
          setOpenEditModal(false)
        }
        onSuccess={loadConsulta}
        consulta={consulta}
      />

      <SolicitacaoExameModal
        open={openSolicitacaoModal}
        consultaId={consulta.id}
        especialidade={especialidade}
        onClose={() =>
          setOpenSolicitacaoModal(false)
        }
        onSuccess={loadSolicitacoesExames}
      />

      <DeclaracaoComparecimentoModal
        open={openDeclaracaoModal}
        consultaId={consulta.id}
        onClose={() =>
          setOpenDeclaracaoModal(false)
        }
        onSuccess={loadDeclaracoes}
      />

    </>
  );
}