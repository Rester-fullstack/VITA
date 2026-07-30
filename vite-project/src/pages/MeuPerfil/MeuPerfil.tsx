import {
  useEffect,
  useState,
  type ChangeEvent
} from "react";

import toast from "react-hot-toast";

import api from "../../api/axios";

import "./MeuPerfil.css";

type Especialidade = {
  id: number;
  nome: string;
};

type PerfilForm = {
  id: number;
  nome: string;
  email: string;
  crm: string;
  especialidadeId: string;
  especialidade: string;
  telefone: string;
  cidade: string;
  estado: string;
  enderecoProfissional: string;
  assinatura: string;
};

const initialForm: PerfilForm = {
  id: 0,
  nome: "",
  email: "",
  crm: "",
  especialidadeId: "",
  especialidade: "",
  telefone: "",
  cidade: "",
  estado: "",
  enderecoProfissional: "",
  assinatura: ""
};

export default function MeuPerfil() {
  const [form, setForm] =
    useState<PerfilForm>(initialForm);

  const [especialidades, setEspecialidades] =
    useState<Especialidade[]>([]);

  const [loading, setLoading] =
    useState(true);

  const [saving, setSaving] =
    useState(false);

  async function load() {
    try {
      setLoading(true);

      const [
        perfilResponse,
        especialidadesResponse
      ] = await Promise.all([
        api.get("/Medicos/meu-perfil"),
        api.get("/Especialidade")
      ]);

      const perfil =
        perfilResponse.data.data;

      const listaEspecialidades =
        especialidadesResponse.data.data ?? [];

      setEspecialidades(
        listaEspecialidades
      );

      setForm({
        id: perfil?.id ?? 0,
        nome: perfil?.nome ?? "",
        email: perfil?.email ?? "",
        crm: perfil?.crm ?? "",
        especialidadeId:
          perfil?.especialidadeId
            ?.toString() ?? "",
        especialidade:
          perfil?.especialidade ?? "",
        telefone:
          perfil?.telefone ?? "",
        cidade:
          perfil?.cidade ?? "",
        estado:
          perfil?.estado ?? "",
        enderecoProfissional:
          perfil?.enderecoProfissional ?? "",
        assinatura:
          perfil?.assinatura ?? ""
      });

    } catch (error: any) {
      console.error(
        "Erro ao carregar perfil:",
        error.response?.data || error
      );

      toast.error(
        error.response?.data?.message ||
        "Erro ao carregar perfil."
      );

    } finally {
      setLoading(false);
    }
  }

  async function salvar() {
    if (!form.nome.trim()) {
      toast.error("Informe o nome.");
      return;
    }

    if (!form.email.trim()) {
      toast.error("Informe o email.");
      return;
    }

    if (!form.crm.trim()) {
      toast.error("Informe o CRM.");
      return;
    }

    if (!form.especialidadeId) {
      toast.error(
        "Selecione uma especialidade."
      );

      return;
    }

    try {
      setSaving(true);

      const response =
        await api.put(
          "/Medicos/meu-perfil",
          {
            nome:
              form.nome.trim(),

            email:
              form.email.trim(),

            crm:
              form.crm.trim(),

            especialidadeId:
              Number(form.especialidadeId),

            telefone:
              form.telefone.trim(),

            cidade:
              form.cidade.trim(),

            estado:
              form.estado.trim(),

            enderecoProfissional:
              form.enderecoProfissional.trim(),

            assinatura:
              form.assinatura.trim()
          }
        );

      const perfilAtualizado =
        response.data.data;

      setForm({
        id:
          perfilAtualizado?.id ??
          form.id,

        nome:
          perfilAtualizado?.nome ??
          form.nome,

        email:
          perfilAtualizado?.email ??
          form.email,

        crm:
          perfilAtualizado?.crm ??
          form.crm,

        especialidadeId:
          perfilAtualizado
            ?.especialidadeId
            ?.toString() ??
          form.especialidadeId,

        especialidade:
          perfilAtualizado
            ?.especialidade ??
          form.especialidade,

        telefone:
          perfilAtualizado
            ?.telefone ??
          form.telefone,

        cidade:
          perfilAtualizado
            ?.cidade ??
          form.cidade,

        estado:
          perfilAtualizado
            ?.estado ??
          form.estado,

        enderecoProfissional:
          perfilAtualizado
            ?.enderecoProfissional ??
          form.enderecoProfissional,

        assinatura:
          perfilAtualizado
            ?.assinatura ??
          form.assinatura
      });

      atualizarUsuarioLocal(
        perfilAtualizado
      );

      toast.success(
        "Perfil atualizado com sucesso."
      );

    } catch (error: any) {
      console.error(
        "Erro ao salvar perfil:",
        error.response?.data || error
      );

      toast.error(
        error.response?.data?.message ||
        "Erro ao salvar perfil."
      );

    } finally {
      setSaving(false);
    }
  }

  function atualizarUsuarioLocal(
    perfilAtualizado: any
  ) {
    const usuarioSalvo =
      JSON.parse(
        localStorage.getItem("user") ||
        "{}"
      );

    const usuarioAtualizado = {
      ...usuarioSalvo,

      nome:
        perfilAtualizado?.nome ??
        form.nome,

      email:
        perfilAtualizado?.email ??
        form.email,

      crm:
        perfilAtualizado?.crm ??
        form.crm,

      especialidade:
        perfilAtualizado
          ?.especialidade ??
        form.especialidade
    };

    localStorage.setItem(
      "user",
      JSON.stringify(
        usuarioAtualizado
      )
    );
  }

  function handleChange(
    event: ChangeEvent<
      HTMLInputElement |
      HTMLTextAreaElement |
      HTMLSelectElement
    >
  ) {
    const {
      name,
      value
    } = event.target;

    if (name === "estado") {
      setForm(current => ({
        ...current,
        estado:
          value
            .toUpperCase()
            .slice(0, 2)
      }));

      return;
    }

    if (name === "especialidadeId") {
      const especialidadeSelecionada =
        especialidades.find(
          especialidade =>
            especialidade.id ===
            Number(value)
        );

      setForm(current => ({
        ...current,
        especialidadeId: value,
        especialidade:
          especialidadeSelecionada?.nome ??
          ""
      }));

      return;
    }

    setForm(current => ({
      ...current,
      [name]: value
    }));
  }

  useEffect(() => {
    load();
  }, []);

  if (loading) {
    return (
      <div className="perfil-loading">
        <div className="perfil-spinner" />

        <span>
          Carregando perfil...
        </span>
      </div>
    );
  }

  const inicial =
    form.nome
      .trim()
      .charAt(0)
      .toUpperCase() || "M";

  return (
    <main className="perfil-page">

      <div className="perfil-container">

        <header className="perfil-page-header">

          <div>
            <span className="perfil-label">
              Área profissional
            </span>

            <h1>
              Meu Perfil
            </h1>

            <p>
              Gerencie seus dados profissionais
              usados no sistema e nos documentos.
            </p>
          </div>

        </header>

        <section className="perfil-card">

          <div className="perfil-profile-header">

            <div className="perfil-avatar">
              {inicial}
            </div>

            <div className="perfil-profile-info">

              <h2>
                {form.nome ||
                  "Médico"}
              </h2>

              <span>
                {form.crm
                  ? `CRM ${form.crm}`
                  : "CRM não informado"}
              </span>

              <small>
                {form.especialidade ||
                  "Especialidade não informada"}
              </small>

            </div>

          </div>

          <div className="perfil-divider" />

          <section className="perfil-section">

            <div className="perfil-section-header">

              <h2>
                Dados Profissionais
              </h2>

              <p>
                Informações principais da sua
                identificação profissional.
              </p>

            </div>

            <div className="perfil-grid">

              <div className="perfil-field">

                <label htmlFor="nome">
                  Nome completo
                </label>

                <input
                  id="nome"
                  name="nome"
                  value={form.nome}
                  onChange={handleChange}
                  placeholder="Digite seu nome"
                />

              </div>

              <div className="perfil-field">

                <label htmlFor="email">
                  Email
                </label>

                <input
                  id="email"
                  name="email"
                  type="email"
                  value={form.email}
                  onChange={handleChange}
                  placeholder="email@exemplo.com"
                />

              </div>

              <div className="perfil-field">

                <label htmlFor="crm">
                  CRM
                </label>

                <input
                  id="crm"
                  name="crm"
                  value={form.crm}
                  onChange={handleChange}
                  placeholder="Digite o CRM"
                />

              </div>

              <div className="perfil-field">

                <label htmlFor="especialidadeId">
                  Especialidade
                </label>

                <select
                  id="especialidadeId"
                  name="especialidadeId"
                  value={form.especialidadeId}
                  onChange={handleChange}
                >
                  <option value="">
                    Selecione
                  </option>

                  {especialidades.map(
                    especialidade => (
                      <option
                        key={especialidade.id}
                        value={especialidade.id}
                      >
                        {especialidade.nome}
                      </option>
                    )
                  )}

                </select>

              </div>

            </div>

          </section>

          <div className="perfil-divider" />

          <section className="perfil-section">

            <div className="perfil-section-header">

              <h2>
                Contato e localização
              </h2>

              <p>
                Dados de contato e endereço do
                atendimento profissional.
              </p>

            </div>

            <div className="perfil-grid">

              <div className="perfil-field">

                <label htmlFor="telefone">
                  Telefone
                </label>

                <input
                  id="telefone"
                  name="telefone"
                  value={form.telefone}
                  onChange={handleChange}
                  placeholder="(79) 99999-9999"
                />

              </div>

              <div className="perfil-field">

                <label htmlFor="cidade">
                  Cidade
                </label>

                <input
                  id="cidade"
                  name="cidade"
                  value={form.cidade}
                  onChange={handleChange}
                  placeholder="Ex.: Aracaju"
                />

              </div>

              <div className="perfil-field">

                <label htmlFor="estado">
                  Estado
                </label>

                <input
                  id="estado"
                  name="estado"
                  value={form.estado}
                  onChange={handleChange}
                  placeholder="Ex.: SE"
                  maxLength={2}
                />

              </div>

              <div className="perfil-field perfil-field-full">

                <label htmlFor="enderecoProfissional">
                  Endereço profissional
                </label>

                <input
                  id="enderecoProfissional"
                  name="enderecoProfissional"
                  value={
                    form.enderecoProfissional
                  }
                  onChange={handleChange}
                  placeholder={
                    "Rua, número, bairro e complemento"
                  }
                />

              </div>

            </div>

          </section>

          <div className="perfil-divider" />

          <section className="perfil-section">

            <div className="perfil-section-header">

              <h2>
                Assinatura profissional
              </h2>

              <p>
                Texto usado na identificação
                dos documentos emitidos.
              </p>

            </div>

            <div className="perfil-field">

              <label htmlFor="assinatura">
                Texto da assinatura
              </label>

              <textarea
                id="assinatura"
                name="assinatura"
                value={form.assinatura}
                onChange={handleChange}
                placeholder={
                  "Ex.: Dr. João Silva - CRM 12345"
                }
                maxLength={500}
              />

              <span className="perfil-character-count">
                {form.assinatura.length}/500
              </span>

            </div>

          </section>

          <div className="perfil-actions">

            <button
              type="button"
              className="perfil-save-button"
              onClick={salvar}
              disabled={saving}
            >
              {saving
                ? "Salvando..."
                : "Salvar perfil"}
            </button>

          </div>

        </section>

      </div>

    </main>
  );
}