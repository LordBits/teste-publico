CREATE TABLE Clientes (
    Codigo INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
    Fantasia NVARCHAR(100) NOT NULL,
    Documento NVARCHAR(20) NOT NULL,
    Endereco NVARCHAR(200) NOT NULL,
    DataCadastro DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Produtos (
    Codigo INT IDENTITY(1,1) PRIMARY KEY,
    Descricao NVARCHAR(150) NOT NULL,
    CodigoBarra NVARCHAR(20) NOT NULL,
    ValorVenda DECIMAL(18,2) NOT NULL,
    PesoBruto DECIMAL(18,3) NOT NULL,
    PesoLiquido DECIMAL(18,3) NOT NULL,
    DataCadastro DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    SenhaHash NVARCHAR(200) NOT NULL,
    DataCadastro DATETIME NOT NULL DEFAULT GETDATE(),
    UltimoLogin DATETIME NULL
);

INSERT INTO Usuarios (Nome, Email, SenhaHash, DataCadastro, UltimoLogin) -- Aqui a senha é 123456, use um gerador se quiser outra senha
VALUES (
    'Admin',
    'teste@teste.com.br',
    '$2a$11$EixZaYVK1fsbw1ZfbX3OXePaWxn96p36bLnz1i3XAkuo/5tXG4Zz2',
    GETDATE(),
    NULL
);