-- ============================================================================
-- SCRIPT DE CRÉATION ET D'INITIALISATION : ParcInformatiqueDB
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ParcInformatiqueDB')
BEGIN
    CREATE DATABASE ParcInformatiqueDB;
END
GO

USE ParcInformatiqueDB;
GO

-- 1. Table USERS
IF OBJECT_ID('Users', 'U') IS NULL
BEGIN
    CREATE TABLE Users (
        IdUser INT IDENTITY(1,1) PRIMARY KEY,
        Username NVARCHAR(50) NOT NULL UNIQUE,
        MotDePasse NVARCHAR(255) NOT NULL,
        Role NVARCHAR(30) NOT NULL, -- Responsable, Technicien, Employé
        StatutCompte NVARCHAR(20) DEFAULT 'Actif'
    );
END
GO

-- 2. Table EMPLOYES
IF OBJECT_ID('Employes', 'U') IS NULL
BEGIN
    CREATE TABLE Employes (
        IdEmploye INT IDENTITY(1,1) PRIMARY KEY,
        Nom NVARCHAR(50) NOT NULL,
        Prenom NVARCHAR(50) NOT NULL,
        Email NVARCHAR(100),
        Departement NVARCHAR(50),
        IdUser INT NULL UNIQUE,
        CONSTRAINT FK_Employes_Users FOREIGN KEY (IdUser) REFERENCES Users(IdUser) ON DELETE SET NULL
    );
END
GO

-- 3. Table TYPES_EQUIPEMENT
IF OBJECT_ID('TypesEquipement', 'U') IS NULL
BEGIN
    CREATE TABLE TypesEquipement (
        IdType INT IDENTITY(1,1) PRIMARY KEY,
        Libelle NVARCHAR(50) NOT NULL
    );
END
GO

-- 4. Table LOCALISATIONS
IF OBJECT_ID('Localisations', 'U') IS NULL
BEGIN
    CREATE TABLE Localisations (
        IdLocalisation INT IDENTITY(1,1) PRIMARY KEY,
        NomEmplacement NVARCHAR(100) NOT NULL
    );
END
GO

-- 5. Table EQUIPEMENTS
IF OBJECT_ID('Equipements', 'U') IS NULL
BEGIN
    CREATE TABLE Equipements (
        IdEquipement INT IDENTITY(1,1) PRIMARY KEY,
        NomEquipement NVARCHAR(100) NOT NULL,
        NumeroSerie NVARCHAR(50) UNIQUE,
        Etat NVARCHAR(30) DEFAULT 'En service',
        IdType INT NOT NULL,
        IdLocalisation INT NOT NULL,
        CONSTRAINT FK_Equipements_Types FOREIGN KEY (IdType) REFERENCES TypesEquipement(IdType),
        CONSTRAINT FK_Equipements_Localisations FOREIGN KEY (IdLocalisation) REFERENCES Localisations(IdLocalisation)
    );
END
GO

-- 6. Table AFFECTATIONS
IF OBJECT_ID('Affectations', 'U') IS NULL
BEGIN
    CREATE TABLE Affectations (
        IdAffectation INT IDENTITY(1,1) PRIMARY KEY,
        DateDebut DATETIME NOT NULL DEFAULT GETDATE(),
        DateFin DATETIME NULL,
        IdEquipement INT NOT NULL,
        IdEmploye INT NOT NULL,
        CONSTRAINT FK_Affectations_Equipements FOREIGN KEY (IdEquipement) REFERENCES Equipements(IdEquipement),
        CONSTRAINT FK_Affectations_Employes FOREIGN KEY (IdEmploye) REFERENCES Employes(IdEmploye)
    );
END
GO

-- 7. Table TICKETS
IF OBJECT_ID('Tickets', 'U') IS NULL
BEGIN
    CREATE TABLE Tickets (
        IdTicket INT IDENTITY(1,1) PRIMARY KEY,
        NomTicket NVARCHAR(100) NOT NULL,
        Description NVARCHAR(MAX) NOT NULL,
        Priorite NVARCHAR(20) DEFAULT 'Moyenne',
        Statut NVARCHAR(30) DEFAULT 'En attente',
        DateCreation DATETIME DEFAULT GETDATE(),
        DateCloture DATETIME NULL,
        Diagnostic NVARCHAR(MAX) NULL,
        TypeIntervention NVARCHAR(50) NULL,
        ActionRealisee NVARCHAR(MAX) NULL,
        IdUserCreateur INT NOT NULL,
        IdUserTraiteur INT NULL,
        IdEquipement INT NOT NULL,
        CONSTRAINT FK_Tickets_Createur FOREIGN KEY (IdUserCreateur) REFERENCES Users(IdUser),
        CONSTRAINT FK_Tickets_Traiteur FOREIGN KEY (IdUserTraiteur) REFERENCES Users(IdUser),
        CONSTRAINT FK_Tickets_Equipement FOREIGN KEY (IdEquipement) REFERENCES Equipements(IdEquipement)
    );
END
GO

-- 8. Table LOGICIELS
IF OBJECT_ID('Logiciels', 'U') IS NULL
BEGIN
    CREATE TABLE Logiciels (
        IdLogiciel INT IDENTITY(1,1) PRIMARY KEY,
        NomLogiciel NVARCHAR(100) NOT NULL,
        Version NVARCHAR(50) NOT NULL,
        Licence NVARCHAR(100) NOT NULL,
        DateExpiration DATETIME NOT NULL
    );
END
GO

-- 9. Table INSTALLATIONS_LOGICIELS
IF OBJECT_ID('InstallationsLogiciels', 'U') IS NULL
BEGIN
    CREATE TABLE InstallationsLogiciels (
        IdInstallation INT IDENTITY(1,1) PRIMARY KEY,
        DateInstallation DATETIME DEFAULT GETDATE(),
        VersionInstallee NVARCHAR(50) NOT NULL,
        IdLogiciel INT NOT NULL,
        IdEquipement INT NOT NULL,
        CONSTRAINT FK_Installations_Logiciels FOREIGN KEY (IdLogiciel) REFERENCES Logiciels(IdLogiciel) ON DELETE CASCADE,
        CONSTRAINT FK_Installations_Equipements FOREIGN KEY (IdEquipement) REFERENCES Equipements(IdEquipement) ON DELETE CASCADE
    );
END
GO

-- ============================================================================
-- JEU DE DONNÉES DE TEST (INITIALISATION)
-- ============================================================================

-- Compte Administrateur / Responsable par défaut (Mot de passe: Admin123!)
IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'admin')
BEGIN
    INSERT INTO Users (Username, MotDePasse, Role, StatutCompte)
    VALUES ('admin', '$2a$11$q9hKk8uV/S13k.0fG/5e.e89xVqV5kK2T2N8O.9KxR9o0Y5vY2W2K', 'Responsable', 'Actif');
END
GO