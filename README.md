# 🏥 VITA - Sistema de Gestão Clínica Inteligente

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet)
![React](https://img.shields.io/badge/React-18-61DAFB?style=for-the-badge&logo=react)
![TypeScript](https://img.shields.io/badge/TypeScript-5-3178C6?style=for-the-badge&logo=typescript)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?style=for-the-badge&logo=microsoftsqlserver)
![JWT](https://img.shields.io/badge/JWT-Authentication-black?style=for-the-badge&logo=jsonwebtokens)

Sistema web completo para gerenciamento de clínicas e profissionais da saúde, desenvolvido utilizando **ASP.NET Core Web API (.NET 8)**, **React + TypeScript**, **SQL Server** e autenticação **JWT**.

# 🎯 Objetivo

O VITA é uma plataforma SaaS desenvolvida para auxiliar profissionais da área da saúde na gestão completa de clínicas e consultórios.

O sistema permite controlar pacientes, consultas, exames, históricos clínicos, receitas médicas e especialidades em uma única plataforma moderna e segura.

---

# 🚀 Tecnologias

## Backend

- ASP.NET Core Web API (.NET 8)
- Entity Framework Core
- SQL Server
- JWT Authentication
- AutoMapper
- FluentValidation
- BCrypt
- Swagger

## Frontend

- React
- TypeScript
- Vite
- Axios
- React Router
- CSS3

---

# 🏗️ Arquitetura

![Arquitetura](docs/diagrams/arquitetura.png)

---

## 🗄️ Modelo de Dados

![DER](docs/diagrams/der.png)

---

# ✨ Funcionalidades

## Administrador

- Login com JWT
- Dashboard administrativo
- Cadastro de médicos
- Aprovação de médicos
- Gerenciamento de pacientes
- Gerenciamento de consultas
- Gerenciamento de exames
- Histórico clínico
- Controle de especialidades

## Médico

- Login seguro
- Dashboard personalizado
- Visualização de pacientes
- Consultas
- Solicitação de exames
- Histórico clínico

---

# 🩺 Especialidades

- Odontologia
- Psicologia
- Nutrição

---

# 📷 Telas do Sistema

## Login

![Login](docs/images/Login.png)

---

## Dashboard Administrador

![Dashboard Admin](docs/images/Admin/dashboard-admin-1.png)


![Dashboard Admin](docs/images/Admin/dashboard-admin-2.png)

---

## Dashboard Médico

![Dashboard Médico](docs/images/Medico/dashboard-medico-1.png)


![Dashboard Médico](docs/images/Medico/dashboard-medico-2.png)


![Dashboard Médico](docs/images/Medico/ajuda-suporte.png)

---

## Pacientes

![Pacientes](docs/images/Medico/pacientes.png)


![Pacientes](docs/images/Medico/novo-paciente.png)

---

## Consultas

![Consultas](docs/images/Medico/consultas.png)


![Consultas](docs/images/Medico/agenda-medica.png)


---

## Exames

![Exames](docs/images/Medico/cadastro-exame.png)



![Exames](docs/images/Medico/lista-exames.png)

---

## Histórico Clínico

![Histórico](docs/images/Medico/historico-clinico.png)

---

## Cadastro de Médicos

![Médicos](docs/images/Admin/cadastro-medico-1.png)


![Médicos](docs/images/Admin/cadastro-medico-2.png)


---

## Área do Profissional

![Área do Profissional](docs/images/Medico/area-profissional.png)

---


# 🔐 Autenticação

O sistema utiliza autenticação baseada em **JWT (JSON Web Token)**.

Recursos implementados:

- Login seguro
- Autorização por perfis
- Tokens JWT
- Refresh Token
- Controle de permissões

---

# 📂 Estrutura do Projeto

```
VITA
│
├── VitaApi
│   ├── Controllers
│   ├── DTOs
│   ├── Models
│   ├── Services
│   ├── Repositories
│   ├── Data
│   └── Program.cs
│
├── vita-project
│   ├── src
│   ├── components
│   ├── pages
│   ├── services
│   └── assets
│
├── docs
│   └── images
│
└── README.md
```

---

# ⚙️ Como Executar

## Backend

Clone o projeto

```bash
git clone https://github.com/SEU-USUARIO/VITA.git

cd VITA
```

Entre na API

```bash
cd VitaApi
```

Configure o **User Secrets**:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "SUA_CONNECTION_STRING"
  },
  "Jwt": {
    "Key": "SUA_CHAVE_JWT"
  }
}
```

Execute:

```bash
dotnet restore
dotnet ef database update
dotnet run
```

---

## Frontend

Entre na pasta do frontend

```bash
cd vita-project
```

Instale as dependências

```bash
npm install
```

Crie um arquivo `.env`:

```env
VITE_API_URL=http://localhost:5182/api
```

Execute:

```bash
npm run dev
```

---

# 📌 Próximas Funcionalidades

- Upload de imagens
- Dashboard com gráficos
- Notificações em tempo real
- Agendamento online
- Relatórios em PDF
- Deploy em nuvem

---

# 👩‍💻 Desenvolvido por

**Ester da Costa Batista**

Desenvolvedora Full Stack

- C#
- .NET
- React
- TypeScript
- SQL Server

GitHub:

https://github.com/Rester-fullstack


---

# 📄 Licença

Este projeto foi desenvolvido para fins de estudo, aprendizado e demonstração de portfólio.