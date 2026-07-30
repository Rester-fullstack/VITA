import {
  useEffect,
  useState
} from "react";

import {
  BookOpen,
  CalendarDays,
  ChevronDown,
  CircleHelp,
  FileText,
  Headphones,
  Mail,
  MessageCircle,
  Phone,
  Search,
  ShieldCheck,
  Stethoscope,
  Users
} from "lucide-react";

import toast from "react-hot-toast";

import api from "../../api/axios";

import "./Ajuda.css";

type ConfiguracaoSuporte = {
  nomePlataforma: string;
  emailSuporte: string;
  telefoneSuporte: string;
  whatsappSuporte: string;
  mensagemPadrao: string;
};

type PerguntaFrequente = {
  id: number;
  pergunta: string;
  resposta: string;
  categoria: string;
};

const perguntasFrequentes: PerguntaFrequente[] = [
  {
    id: 1,
    categoria: "Consultas",
    pergunta: "Como criar uma nova consulta?",
    resposta:
      "Acesse a página Consultas, clique em Nova Consulta, selecione o paciente, o médico, a data e preencha as informações necessárias."
  },
  {
    id: 2,
    categoria: "Consultas",
    pergunta: "Por que não consigo excluir uma consulta?",
    resposta:
      "A exclusão permanente só é permitida para consultas agendadas que ainda não possuam histórico clínico, receitas, exames, atestados, solicitações ou declarações vinculadas."
  },
  {
    id: 3,
    categoria: "Pacientes",
    pergunta: "Como cadastrar um paciente?",
    resposta:
      "Acesse a página Pacientes e clique em Novo Paciente. Preencha os dados obrigatórios e confirme o cadastro."
  },
  {
    id: 4,
    categoria: "Documentos",
    pergunta: "Como emitir uma receita ou atestado?",
    resposta:
      "Abra a consulta do paciente e acesse a área de documentos. Selecione o tipo de documento, preencha os dados e gere o PDF."
  },
  {
    id: 5,
    categoria: "Exames",
    pergunta: "Como anexar um exame?",
    resposta:
      "Acesse a área de exames, selecione a consulta correspondente e envie o arquivo permitido pelo sistema."
  },
  {
    id: 6,
    categoria: "Segurança",
    pergunta: "Meus dados estão protegidos?",
    resposta:
      "O VITA utiliza autenticação, controle de acesso por perfil e registro de auditoria das principais ações realizadas no sistema."
  }
];

export default function Ajuda() {
  const [configuracao, setConfiguracao] =
    useState<ConfiguracaoSuporte>({
      nomePlataforma: "VITA",
      emailSuporte: "",
      telefoneSuporte: "",
      whatsappSuporte: "",
      mensagemPadrao: ""
    });

  const [loading, setLoading] =
    useState(true);

  const [search, setSearch] =
    useState("");

  const [openQuestionId, setOpenQuestionId] =
    useState<number | null>(null);

  async function carregarConfiguracoes() {
    try {
      const response =
        await api.get("/ConfiguracaoClinica");

      const config =
        response.data.data;

      setConfiguracao({
        nomePlataforma:
          config?.nomePlataforma ?? "VITA",

        emailSuporte:
          config?.emailSuporte ?? "",

        telefoneSuporte:
          config?.telefoneSuporte ?? "",

        whatsappSuporte:
          config?.whatsappSuporte ?? "",

        mensagemPadrao:
          config?.mensagemPadrao ?? ""
      });

    } catch (error) {
      console.error(error);

      toast.error(
        "Não foi possível carregar os dados de suporte."
      );

    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    carregarConfiguracoes();
  }, []);

  const perguntasFiltradas =
    perguntasFrequentes.filter(item => {
      const termo =
        search.trim().toLowerCase();

      if (!termo) {
        return true;
      }

      return (
        item.pergunta
          .toLowerCase()
          .includes(termo) ||

        item.resposta
          .toLowerCase()
          .includes(termo) ||

        item.categoria
          .toLowerCase()
          .includes(termo)
      );
    });

  function abrirWhatsapp() {
    if (!configuracao.whatsappSuporte) {
      toast.error(
        "O WhatsApp de suporte ainda não foi configurado."
      );

      return;
    }

    const numero =
      configuracao.whatsappSuporte
        .replace(/\D/g, "");

    const mensagem =
      encodeURIComponent(
        `Olá, preciso de ajuda com a plataforma ${configuracao.nomePlataforma}.`
      );

    window.open(
      `https://wa.me/55${numero}?text=${mensagem}`,
      "_blank",
      "noopener,noreferrer"
    );
  }

  function abrirEmail() {
    if (!configuracao.emailSuporte) {
      toast.error(
        "O email de suporte ainda não foi configurado."
      );

      return;
    }

    const assunto =
      encodeURIComponent(
        `Suporte - ${configuracao.nomePlataforma}`
      );

    window.location.href =
      `mailto:${configuracao.emailSuporte}?subject=${assunto}`;
  }

  if (loading) {
    return (
      <div className="help-loading">
        Carregando central de ajuda...
      </div>
    );
  }

  return (
    <main className="help-page">

      <header className="help-header">

        <div className="help-header-content">

          <span className="help-eyebrow">
            Central de suporte
          </span>

          <h1>
            Como podemos ajudar?
          </h1>

          <p>
            Encontre orientações sobre o uso do sistema
            ou entre em contato com o suporte.
          </p>

          <div className="help-search">

            <Search size={20} />

            <input
              type="text"
              placeholder="Pesquise uma dúvida..."
              value={search}
              onChange={event =>
                setSearch(event.target.value)
              }
            />

          </div>

        </div>

        <div className="help-header-icon">
          <CircleHelp size={42} />
        </div>

      </header>

      {configuracao.mensagemPadrao && (
        <section className="help-message">

          <MessageCircle size={21} />

          <div>
            <strong>
              Mensagem da plataforma
            </strong>

            <p>
              {configuracao.mensagemPadrao}
            </p>
          </div>

        </section>
      )}

      <section className="help-quick-grid">

        <article className="help-quick-card">
          <div className="help-card-icon">
            <Users size={22} />
          </div>

          <h2>
            Pacientes
          </h2>

          <p>
            Cadastre pacientes e consulte seus
            dados clínicos.
          </p>
        </article>

        <article className="help-quick-card">
          <div className="help-card-icon">
            <CalendarDays size={22} />
          </div>

          <h2>
            Consultas
          </h2>

          <p>
            Agende, edite, visualize e acompanhe
            consultas.
          </p>
        </article>

        <article className="help-quick-card">
          <div className="help-card-icon">
            <Stethoscope size={22} />
          </div>

          <h2>
            Atendimento
          </h2>

          <p>
            Registre históricos e informações
            clínicas do atendimento.
          </p>
        </article>

        <article className="help-quick-card">
          <div className="help-card-icon">
            <FileText size={22} />
          </div>

          <h2>
            Documentos
          </h2>

          <p>
            Gere receitas, atestados, declarações
            e solicitações.
          </p>
        </article>

      </section>

      <div className="help-content-grid">

        <section className="help-section">

          <div className="help-section-header">

            <div>
              <span>
                Dúvidas comuns
              </span>

              <h2>
                Perguntas frequentes
              </h2>
            </div>

            <BookOpen size={25} />

          </div>

          <div className="help-faq-list">

            {perguntasFiltradas.length === 0 ? (

              <div className="help-empty">
                Nenhuma dúvida encontrada.
              </div>

            ) : (

              perguntasFiltradas.map(item => {

                const isOpen =
                  openQuestionId === item.id;

                return (
                  <article
                    key={item.id}
                    className={`help-faq-item ${
                      isOpen ? "open" : ""
                    }`}
                  >

                    <button
                      type="button"
                      className="help-faq-question"
                      onClick={() =>
                        setOpenQuestionId(
                          isOpen ? null : item.id
                        )
                      }
                    >
                      <div>
                        <small>
                          {item.categoria}
                        </small>

                        <strong>
                          {item.pergunta}
                        </strong>
                      </div>

                      <ChevronDown size={20} />
                    </button>

                    {isOpen && (
                      <div className="help-faq-answer">
                        <p>
                          {item.resposta}
                        </p>
                      </div>
                    )}

                  </article>
                );
              })

            )}

          </div>

        </section>

        <aside className="help-support-card">

          <div className="help-support-icon">
            <Headphones size={27} />
          </div>

          <h2>
            Fale com o suporte
          </h2>

          <p>
            Não encontrou o que precisava?
            Entre em contato pelos canais disponíveis.
          </p>

          <div className="help-support-list">

            <button
              type="button"
              onClick={abrirEmail}
              disabled={!configuracao.emailSuporte}
            >
              <Mail size={19} />

              <div>
                <span>
                  Email
                </span>

                <strong>
                  {configuracao.emailSuporte ||
                    "Não configurado"}
                </strong>
              </div>
            </button>

            <a
              href={
                configuracao.telefoneSuporte
                  ? `tel:${configuracao.telefoneSuporte}`
                  : undefined
              }
              className={
                !configuracao.telefoneSuporte
                  ? "disabled"
                  : ""
              }
            >
              <Phone size={19} />

              <div>
                <span>
                  Telefone
                </span>

                <strong>
                  {configuracao.telefoneSuporte ||
                    "Não configurado"}
                </strong>
              </div>
            </a>

            <button
              type="button"
              onClick={abrirWhatsapp}
              disabled={!configuracao.whatsappSuporte}
            >
              <MessageCircle size={19} />

              <div>
                <span>
                  WhatsApp
                </span>

                <strong>
                  {configuracao.whatsappSuporte ||
                    "Não configurado"}
                </strong>
              </div>
            </button>

          </div>

          <div className="help-security">

            <ShieldCheck size={19} />

            <p>
              Nunca compartilhe sua senha ou token
              de acesso com outras pessoas.
            </p>

          </div>

        </aside>

      </div>

    </main>
  );
}