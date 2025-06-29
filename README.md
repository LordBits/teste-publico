# Projeto ASP.NET Core - Softline

## 📌 Visão Geral

Este projeto é uma aplicação web construída com **ASP.NET Core MVC**, com autenticação baseada em cookies, controle de sessão e rotas amigáveis (URLs em minúsculas com hífens). Foi planejado para ser **seguro, responsivo, portável e escalável**, pronto para ambientes locais ou hospedagem online via IIS ou outros serviços.

---

## ✨ Funcionalidades

- ✅ Login e logout com gerenciamento seguro de sessão
- 🔐 Redirecionamento automático de usuários não autenticados
- 🌐 URLs amigáveis e otimizadas para SEO
- 🧼 Prevenção de cache em páginas protegidas (impede acesso com botão "voltar")
- 🔔 Mensagens de feedback para interações (ex: erro de login)
- 📱 Interface responsiva com validações no lado cliente
- 🧪 Separação clara de ambientes (`Development`, `Production`, etc.)
- 📚 Documentação de rotas e endpoints com Swagger

---

## 🛠 Tecnologias Utilizadas

- ASP.NET Core MVC
- Entity Framework Core + SQL Server
- JavaScript puro + Bootstrap
- Swagger (OpenAPI)
- IIS (Internet Information Services) como servidor opcional
- NuGet com configuração customizada (`nuget.config`)

---

## 🧩 Estrutura do Projeto

```
/teste
│
├── Teste.sln
├── global.json
├── nuget.config
└── Teste.Web/
    ├── Controllers/
    ├── Views/
    ├── Models/
    ├── appsettings.json
    └── Program.cs
```

---

## 🚀 Como Rodar em Outra Máquina (Ambiente Local)

1. **Pré-requisitos**:
   - [.NET SDK](https://dotnet.microsoft.com/en-us/download) (mesma versão definida no `global.json`)
   - SQL Server (local ou remoto)
   - Visual Studio Code ou Visual Studio
   - IIS + .NET Hosting Bundle (se quiser simular ambiente de produção)

2. **Clonar o repositório**:
   ```bash
   git clone https://github.com/LordBits/teste-publico.git
   cd teste
   ```

3. **Restaurar pacotes** (com nuget.config seguro):
   ```bash
   dotnet restore --configfile nuget.config
   ```

4. **Rodar a aplicação**:
   ```bash
   dotnet run --project Teste.Web
   ```

5. **Acessar**:
   Abra o navegador e vá para `http://localhost:5000` ou a porta definida no `launchSettings.json`.

---

## 💾 Publicação e Hospedagem

> Para simular hospedagem real:

1. Publique o projeto:
   ```bash
   dotnet publish Teste.Web -c Release -o ./publicado
   ```

2. No IIS:
   - Crie um novo site apontando para `./publicado`
   - Use o .NET Hosting Bundle instalado
   - Configure permissões para `IIS_IUSRS`

---

## 🧠 Considerações Finais

- O projeto está pronto para ser executado em múltiplos ambientes, com configuração isolada de pacotes e ambientes.
- URLs e sessões foram cuidadosamente tratadas para maximizar segurança e usabilidade.
- Você pode adaptar o sistema para novos módulos e bancos com facilidade.

---

## 👨‍💻 Autor

Matheus Bispo Domingues

---

## 📄 Licença

Este projeto é de uso pessoal e educacional. Para fins comerciais, entre em contato com o autor.
