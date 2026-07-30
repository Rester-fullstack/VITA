import { motion } from "framer-motion";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

import api from "../../api/axios";
import { saveAuth } from "../../storage/auth";

import "./Login.css";

type ConfiguracaoPlataforma = {
  nomePlataforma: string;
  emailSuporte: string;
  telefoneSuporte: string;
  whatsappSuporte: string;
};

export default function Login() {

  const navigate = useNavigate();

  const [email, setEmail] =
    useState("");

  const [password, setPassword] =
    useState("");

  const [loading, setLoading] =
    useState(false);

  const [configuracao, setConfiguracao] =
    useState<ConfiguracaoPlataforma>({
      nomePlataforma: "VITA",
      emailSuporte: "",
      telefoneSuporte: "",
      whatsappSuporte: ""
    });

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
          config?.whatsappSuporte ?? ""
      });

    } catch (error) {
      console.error(error);
    }

  }

  useEffect(() => {
    carregarConfiguracoes();
  }, []);

  async function handleLogin(
    e: React.FormEvent
  ) {

    e.preventDefault();

    try {

      setLoading(true);

      const response =
        await api.post(
          "/auth/login",
          {
            email,
            password
          }
        );

      const data =
        response.data.data;

      saveAuth(
        data.token,
        data.refreshToken,
        {
          id: data.id,
          nome: data.nome,
          email: data.email,
          role: data.role,
          especialidade: data.especialidade,
          crm: data.crm
        }
      );

      if (data.role === "Admin") {
        navigate("/dashboard");
        return;
      }

      if (data.role === "Medico") {
        navigate("/dashboard-medico");
        return;
      }

      alert("Perfil não autorizado");

    } catch (error) {

      console.error(
        "Erro login:",
        error
      );

      alert("Login inválido");

    } finally {

      setLoading(false);

    }

  }

  return (

    <div className="login-page">

      <motion.div
        className="login-card"
        initial={{
          opacity: 0,
          y: 30
        }}
        animate={{
          opacity: 1,
          y: 0
        }}
        transition={{
          duration: 0.5
        }}
      >

        <div className="login-left">

          <div className="login-header">

            <span>
              {configuracao.nomePlataforma}
            </span>

            <h1>
              Bem-vinda de volta
            </h1>

            <p>
              Sistema clínico inteligente para gestão de
              pacientes, consultas, documentos clínicos
              e atendimento médico.
            </p>

          </div>

          <div className="login-help">

            <h3>
              Precisa de ajuda?
            </h3>

            <p>
              Caso esteja com dificuldades para acessar
              o sistema, utilize um dos canais abaixo.
            </p>

            {configuracao.emailSuporte && (

              <div className="help-item">

                <strong>Email</strong>

                <span>
                  {configuracao.emailSuporte}
                </span>

              </div>

            )}

            {configuracao.telefoneSuporte && (

              <div className="help-item">

                <strong>Telefone</strong>

                <span>
                  {configuracao.telefoneSuporte}
                </span>

              </div>

            )}

            {configuracao.whatsappSuporte && (

              <div className="help-item">

                <strong>WhatsApp</strong>

                <span>
                  {configuracao.whatsappSuporte}
                </span>

              </div>

            )}

          </div>

        </div>

        <div className="login-right">

          <form
            onSubmit={handleLogin}
          >

            <div className="input-group">

              <label>
                Email
              </label>

              <input
                type="email"
                placeholder="Digite seu email"
                value={email}
                onChange={(e) =>
                  setEmail(e.target.value)
                }
                required
              />

            </div>

            <div className="input-group">

              <label>
                Senha
              </label>

              <input
                type="password"
                placeholder="Digite sua senha"
                value={password}
                onChange={(e) =>
                  setPassword(e.target.value)
                }
                required
              />

            </div>

            <button
              type="submit"
              disabled={loading}
            >
              {
                loading
                  ? "Entrando..."
                  : "Entrar"
              }
            </button>

          </form>

        </div>

      </motion.div>

    </div>

  );

}