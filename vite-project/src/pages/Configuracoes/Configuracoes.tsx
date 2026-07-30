import {
  useEffect,
  useState,
  type ChangeEvent
} from "react";

import {
  Headphones,
  MessageSquareText,
  Save,
  Settings,
  FileText
} from "lucide-react";

import toast from "react-hot-toast";

import api from "../../api/axios";

import "./Configuracoes.css";

type ConfiguracaoPlataforma = {
  nomePlataforma: string;
  emailSuporte: string;
  telefoneSuporte: string;
  whatsappSuporte: string;
  rodapePdf: string;
  mensagemPadrao: string;
};

const initialForm: ConfiguracaoPlataforma = {
  nomePlataforma: "VITA",
  emailSuporte: "",
  telefoneSuporte: "",
  whatsappSuporte: "",
  rodapePdf:
    "Documento emitido eletronicamente pela plataforma VITA.",
  mensagemPadrao: ""
};

export default function Configuracoes() {
  const [form, setForm] =
    useState<ConfiguracaoPlataforma>(initialForm);

  const [loading, setLoading] =
    useState(true);

  const [saving, setSaving] =
    useState(false);

  async function carregarConfiguracoes() {
    try {
      const response =
        await api.get("/ConfiguracaoClinica");

      const config =
        response.data.data;

      setForm({
        nomePlataforma:
          config?.nomePlataforma ?? "VITA",

        emailSuporte:
          config?.emailSuporte ?? "",

        telefoneSuporte:
          config?.telefoneSuporte ?? "",

        whatsappSuporte:
          config?.whatsappSuporte ?? "",

        rodapePdf:
          config?.rodapePdf ??
          initialForm.rodapePdf,

        mensagemPadrao:
          config?.mensagemPadrao ?? ""
      });

    } catch (error) {
      console.error(error);

      toast.error(
        "Não foi possível carregar as configurações."
      );

    } finally {
      setLoading(false);
    }
  }

  async function salvarConfiguracoes() {
    if (!form.nomePlataforma.trim()) {
      toast.error(
        "Informe o nome da plataforma."
      );

      return;
    }

    try {
      setSaving(true);

      const response =
        await api.put(
          "/ConfiguracaoClinica",
          form
        );

      const config =
        response.data.data;

      setForm({
        nomePlataforma:
          config?.nomePlataforma ??
          form.nomePlataforma,

        emailSuporte:
          config?.emailSuporte ??
          form.emailSuporte,

        telefoneSuporte:
          config?.telefoneSuporte ??
          form.telefoneSuporte,

        whatsappSuporte:
          config?.whatsappSuporte ??
          form.whatsappSuporte,

        rodapePdf:
          config?.rodapePdf ??
          form.rodapePdf,

        mensagemPadrao:
          config?.mensagemPadrao ??
          form.mensagemPadrao
      });

      toast.success(
        "Configurações salvas com sucesso."
      );

    } catch (error) {
      console.error(error);

      toast.error(
        "Não foi possível salvar as configurações."
      );

    } finally {
      setSaving(false);
    }
  }

  function handleChange(
    event: ChangeEvent<
      HTMLInputElement |
      HTMLTextAreaElement
    >
  ) {
    const {
      name,
      value
    } = event.target;

    setForm(current => ({
      ...current,
      [name]: value
    }));
  }

  useEffect(() => {
    carregarConfiguracoes();
  }, []);

  if (loading) {
    return (
      <div className="config-loading">
        Carregando configurações...
      </div>
    );
  }

  return (
    <main className="config-page">

      <header className="config-page-header">
        <div>
          <span className="config-eyebrow">
            Administração
          </span>

          <h1>
            Configurações da Plataforma
          </h1>

          <p>
            Gerencie as informações institucionais,
            os canais de suporte e os textos utilizados
            nos documentos do VITA.
          </p>
        </div>

        <div className="config-header-icon">
          <Settings size={28} />
        </div>
      </header>

      <section className="config-card">

        <div className="config-section">

          <div className="config-section-title">
            <div className="config-section-icon">
              <Settings size={20} />
            </div>

            <div>
              <h2>
                Identidade da Plataforma
              </h2>

              <p>
                Informações gerais exibidas no sistema.
              </p>
            </div>
          </div>

          <div className="config-grid">

            <div className="config-field config-field-full">
              <label htmlFor="nomePlataforma">
                Nome da plataforma
              </label>

              <input
                id="nomePlataforma"
                name="nomePlataforma"
                value={form.nomePlataforma}
                onChange={handleChange}
                placeholder="Ex.: VITA"
                maxLength={150}
              />
            </div>

          </div>

        </div>

        <div className="config-divider" />

        <div className="config-section">

          <div className="config-section-title">
            <div className="config-section-icon">
              <Headphones size={20} />
            </div>

            <div>
              <h2>
                Canais de Suporte
              </h2>

              <p>
                Contatos utilizados pelos médicos
                para solicitar ajuda.
              </p>
            </div>
          </div>

          <div className="config-grid">

            <div className="config-field">
              <label htmlFor="emailSuporte">
                Email de suporte
              </label>

              <input
                id="emailSuporte"
                name="emailSuporte"
                type="email"
                value={form.emailSuporte}
                onChange={handleChange}
                placeholder="suporte@vita.com"
              />
            </div>

            <div className="config-field">
              <label htmlFor="telefoneSuporte">
                Telefone
              </label>

              <input
                id="telefoneSuporte"
                name="telefoneSuporte"
                value={form.telefoneSuporte}
                onChange={handleChange}
                placeholder="(79) 0000-0000"
              />
            </div>

            <div className="config-field">
              <label htmlFor="whatsappSuporte">
                WhatsApp
              </label>

              <input
                id="whatsappSuporte"
                name="whatsappSuporte"
                value={form.whatsappSuporte}
                onChange={handleChange}
                placeholder="(79) 99999-9999"
              />
            </div>

          </div>

        </div>

        <div className="config-divider" />

        <div className="config-section">

          <div className="config-section-title">
            <div className="config-section-icon">
              <FileText size={20} />
            </div>

            <div>
              <h2>
                PDFs e Documentos
              </h2>

              <p>
                Textos institucionais utilizados
                nos documentos emitidos pelo VITA.
              </p>
            </div>
          </div>

          <div className="config-grid">

            <div className="config-field config-field-full">
              <label htmlFor="rodapePdf">
                Rodapé padrão dos PDFs
              </label>

              <textarea
                id="rodapePdf"
                name="rodapePdf"
                value={form.rodapePdf}
                onChange={handleChange}
                placeholder="Texto exibido no rodapé dos documentos..."
                maxLength={1000}
              />

              <small>
                {form.rodapePdf.length}/1000 caracteres
              </small>
            </div>

            <div className="config-field config-field-full">
              <label htmlFor="mensagemPadrao">
                Mensagem institucional
              </label>

              <textarea
                id="mensagemPadrao"
                name="mensagemPadrao"
                value={form.mensagemPadrao}
                onChange={handleChange}
                placeholder="Mensagem institucional ou orientação padrão..."
                maxLength={1000}
              />

              <small>
                {form.mensagemPadrao.length}/1000 caracteres
              </small>
            </div>

          </div>

        </div>

        <div className="config-divider" />

        <div className="config-preview">

          <div className="config-preview-icon">
            <MessageSquareText size={20} />
          </div>

          <div>
            <strong>
              Prévia institucional
            </strong>

            <p>
              {form.mensagemPadrao ||
                "Nenhuma mensagem institucional configurada."}
            </p>

            <small>
              Suporte:{" "}
              {form.emailSuporte ||
                form.whatsappSuporte ||
                "não informado"}
            </small>
          </div>

        </div>

        <div className="config-actions">
          <button
            type="button"
            className="config-save-btn"
            onClick={salvarConfiguracoes}
            disabled={saving}
          >
            <Save size={18} />

            {saving
              ? "Salvando..."
              : "Salvar configurações"}
          </button>
        </div>

      </section>

    </main>
  );
}