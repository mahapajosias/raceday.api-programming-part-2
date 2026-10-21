
CREATE DATABASE IF NOT EXISTS RaceDay;
USE RaceDay;

CREATE TABLE USERS (
    UserID INT AUTO_INCREMENT PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    Role ENUM('Participant', 'Organiser') NOT NULL,
    Phone VARCHAR(20),
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE EVENTS (
    EventID INT AUTO_INCREMENT PRIMARY KEY,
    OrganiserID INT NOT NULL,
    EventName VARCHAR(100) NOT NULL,
    Description TEXT,
    EventDate DATE NOT NULL,
    Location VARCHAR(150) NOT NULL,
    Distance DECIMAL(6,2),
    EventType VARCHAR(50),

    CONSTRAINT FK_Events_Organiser
        FOREIGN KEY (OrganiserID)
        REFERENCES USERS(UserID)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

CREATE TABLE CATEGORIES (
    CategoryID INT AUTO_INCREMENT PRIMARY KEY,
    EventID INT NOT NULL,
    CategoryName VARCHAR(100) NOT NULL,
    CategoryType VARCHAR(50),
    CategoryValue VARCHAR(50),

    CONSTRAINT FK_Categories_Event
        FOREIGN KEY (EventID)
        REFERENCES EVENTS(EventID)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE TABLE CATEGORIES (
    CategoryID INT AUTO_INCREMENT PRIMARY KEY,
    EventID INT NOT NULL,
    CategoryName VARCHAR(100) NOT NULL,
    CategoryType VARCHAR(50),
    CategoryValue VARCHAR(50),

    CONSTRAINT FK_Categories_Event
        FOREIGN KEY (EventID)
        REFERENCES EVENTS(EventID)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);

CREATE TABLE ENROLMENTS (
    EnrolmentID INT AUTO_INCREMENT PRIMARY KEY,
    ParticipantID INT NOT NULL,
    EventID INT NOT NULL,
    CategoryID INT NOT NULL,
    EnrolmentDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    EnrolmentStatus VARCHAR(30) DEFAULT 'Active',

    CONSTRAINT FK_Enrolments_Participant
        FOREIGN KEY (ParticipantID)
        REFERENCES USERS(UserID)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,

    CONSTRAINT FK_Enrolments_Event
        FOREIGN KEY (EventID)
        REFERENCES EVENTS(EventID)
        ON UPDATE CASCADE
        ON DELETE CASCADE,

    CONSTRAINT FK_Enrolments_Category
        FOREIGN KEY (CategoryID)
        REFERENCES CATEGORIES(CategoryID)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

CREATE TABLE RESULTS (
    ResultID INT AUTO_INCREMENT PRIMARY KEY,
    EnrolmentID INT NOT NULL UNIQUE,
    FinishTime TIME,
    FinishPosition INT,
    ResultDate DATE,

    CONSTRAINT FK_Results_Enrolment
        FOREIGN KEY (EnrolmentID)
        REFERENCES ENROLMENTS(EnrolmentID)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);


CREATE TABLE PARTICIPANT_PROFILES (
    ProfileID INT AUTO_INCREMENT PRIMARY KEY,
    UserID INT NOT NULL UNIQUE,
    DateOfBirth DATE,
    Gender VARCHAR(30),
    ProfilePictureURL VARCHAR(255),
    Address VARCHAR(255),

    CONSTRAINT FK_Profiles_User
        FOREIGN KEY (UserID)
        REFERENCES USERS(UserID)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);