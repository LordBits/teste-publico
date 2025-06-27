# Projeto ASP.NET Core - Softline

## Visão Geral

Este projeto é uma aplicação web construída com ASP.NET Core MVC que inclui autenticação baseada em cookies, controle de sessão e rotas configuradas para URLs amigáveis, com todos os caminhos em letras minúsculas e separação por hífens.

---

## Funcionalidades

- Sistema de login e logout com gerenciamento seguro de sessões.
- Controle de acesso às páginas, garantindo que usuários não autenticados sejam redirecionados.
- URLs consistentes, padronizadas em minúsculas e com nomes separados por hífen para melhor usabilidade e SEO.
- Prevenção de cache de páginas protegidas para impedir o acesso após logout pelo botão "voltar" do navegador.
- Mensagens de feedback para ações do usuário, como erros de login ou notificações após logout.
- Interface responsiva com validações no lado cliente para melhor experiência do usuário.
- Configuração de ambiente para facilitar desenvolvimento e testes, incluindo documentação das rotas e endpoints.

---

## Tecnologias Utilizadas

- ASP.NET Core MVC
- Entity Framework Core com SQL Server
- JavaScript para validações no frontend
- Bootstrap para estilização

---

## Considerações Gerais

- O projeto foi estruturado visando manutenibilidade e escalabilidade futura.
- As rotas e URLs foram configuradas para melhorar a experiência do usuário e favorecer práticas recomendadas de SEO.
- O sistema de autenticação foi desenvolvido para garantir segurança e simplicidade na navegação.
- O cache das páginas protegidas é desabilitado para aumentar a segurança e evitar inconsistências de sessão.

---

## Como usar

Basta rodar a aplicação em ambiente configurado com SQL Server, acessar via navegador e interagir com o sistema de autenticação e páginas protegidas.

