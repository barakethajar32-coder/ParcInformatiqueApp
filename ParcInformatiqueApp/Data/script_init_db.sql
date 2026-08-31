-- SCRIPT DE CRÉATION ET D'INITIALISATION : ParcInformatiqueDB

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

-- JEU DE DONNÉES DE TEST (INITIALISATION)
-- 1. INSERTION DES SERVICES
INSERT INTO [SERVICES] ([NomService]) VALUES
(N'Direction Générale'),
(N'Systèmes d''Information'),
(N'Ressources Humaines'),
(N'Comptabilité et Finance');
GO

-- 2. INSERTION DES LOCALISATIONS
INSERT INTO [LOCALISATIONS] ([NomEmplacement]) VALUES
(N'Bureau 101 - RDC'),
(N'Bureau 202 - Etage 1'),
(N'Salle Serveur - Sous-sol'),
(N'Open Space - Etage 2');
GO

-- 3. INSERTION DES TYPES D'ÉQUIPEMENTS
INSERT INTO [TYPE_EQUIPEMENTS] ([Libelle]) VALUES
(N'Ordinateur Portable'),
(N'Ordinateur de Bureau'),
(N'Serveur Physiques'),
(N'Imprimante Réseau');
GO

-- 4. INSERTION DES EMPLOYES
-- Service 1: Direction, 2: DSI, 3: RH, 4: Compta
INSERT INTO [EMPLOYES] ([Nom], [Prenom], [Email], [IdService]) VALUES
(N'El Amrani', N'Youssef', N'y.amrani@entreprise.ma', 2), -- Admin DSI
(N'Benali', N'Khadija', N'k.benali@entreprise.ma', 2),  -- Technicien DSI
(N'Chraibi', N'Omar', N'o.chraibi@entreprise.ma', 3),     -- User RH
(N'Bennani', N'Salma', N's.bennani@entreprise.ma', 4);    -- User Compta
GO

-- 5. INSERTION DES UTILISATEURS (USERS)
-- Mots de passe hashés (Exemple générique / Admin123!)
INSERT INTO [USERS] ([Login], [MotDePasse], [Role], [StatutCompte], [IdEmploye]) VALUES
(N'admin', N'$2a$11$q9hKk8uV/S13k.0fG/5e.e89xVqV5kK2', N'Responsable', N'Actif', 1),
(N'tech_benali', N'$2a$11$q9hKk8uV/S13k.0fG/5e.e89xVqV5kK2', N'Technicien', N'Actif', 2),
(N'emp_chraibi', N'$2a$11$q9hKk8uV/S13k.0fG/5e.e89xVqV5kK2', N'Employe', N'Actif', 3);
GO

-- 6. INSERTION DES ÉQUIPEMENTS
INSERT INTO [EQUIPEMENTS] ([NomEquipement], [NumeroSerie], [Etat], [IdType], [IdLocalisation]) VALUES
(N'Dell Latitude 5520', N'SN-DELL-2026-001', N'En service', 1, 2),
(N'HP EliteDesk 800', N'SN-HP-2026-002', N'En service', 2, 4),
(N'Serveur Dell PowerEdge R740', N'SN-SRV-2026-003', N'En service', 3, 3),
(N'Imprimante HP LaserJet Pro', N'SN-IMP-2026-004', N'En panne', 4, 1);
GO

-- 7. INSERTION DES LOGICIELS
INSERT INTO [LOGICIELS] ([NomLogiciel], [Version], [Licence], [DateExpiration]) VALUES
(N'Microsoft Office 2021', N'16.0', N'Pro Plus Enterprise', '2027-12-31'),
(N'Kaspersky Endpoint Security', N'11.6', N'KAS-8899-BUS', '2026-11-30'),
(N'Visual Studio Professional', N'2022', N'VS-PRO-2022-LIC', '2028-05-15');
GO

-- 8. INSERTION DES AFFECTATIONS
INSERT INTO [AFFECTATIONS] ([DateDebut], [DateFin], [IdEquipement], [IdEmploye]) VALUES
('2026-01-10', NULL, 1, 3), -- Laptop affecté à Omar Chraibi (RH)
('2026-02-01', NULL, 2, 4); -- PC Fixe affecté à Salma Bennani (Compta)
GO

-- 9. INSERTION DES INSTALLATIONS LOGICIELS
INSERT INTO [INSTALLATIONS_LOGICIELS] ([DateInstallation], [VersionInstallee], [IdLogiciel], [IdEquipement]) VALUES
('2026-01-11', N'16.0', 1, 1), -- Office sur Laptop RH
('2026-01-11', N'11.6', 2, 1), -- Antivirus sur Laptop RH
('2026-02-02', N'16.0', 1, 2); -- Office sur PC Compta
GO

-- 10. INSERTION DES TICKETS
INSERT INTO [TICKETS] 
([NomTicket], [Description], [Priorite], [Statut], [DateCreation], [DateCloture], [Diagnostic], [TypeIntervention], [ActionRealisee], [IdUserCreateur], [IdUserTraiteur], [IdEquipement]) 
VALUES
(
    N'Imprimante ne s''allume plus', 
    N'L''imprimante du bureau 101 ne répond plus suite à une coupure d''électricité.', 
    N'Haute', 
    N'En cours', 
    '2026-08-25 09:30:00', 
    NULL, 
    N'Bloc d''alimentation grillé.', 
    N'Matériel', 
    N'Commande d''un nouveau bloc d''alimentation en cours.', 
    3, -- Créé par emp_chraibi
    2, -- Pris en charge par tech_benali
    4  -- Concerne l'imprimante HP
),
(
    N'Problème d''activation d''Office', 
    N'Message d''erreur de licence expirée au lancement d''Excel.', 
    N'Moyenne', 
    N'Fermé', 
    '2026-08-20 14:00:00', 
    '2026-08-20 15:30:00', 
    N'Clé de produit non renseignée correctement.', 
    N'Logiciel', 
    N'Réactivation de la licence via le serveur KMS local.', 
    3, -- Créé par emp_chraibi
    1, -- Traité par admin
    1  -- Concerne le Laptop Dell
);
END
GO