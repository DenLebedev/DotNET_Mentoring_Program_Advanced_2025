﻿-- Create the database
CREATE DATABASE IF NOT EXISTS CatalogServiceDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE CatalogServiceDB;

-- Create Category table
CREATE TABLE category (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    ImageUrl VARCHAR(255),
    ParentCategoryId INT NULL,
    FOREIGN KEY (ParentCategoryId) REFERENCES category(Id) ON DELETE RESTRICT
);

-- Create Product table
CREATE TABLE product (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    Description TEXT,
    ImageUrl VARCHAR(255),
    Price DECIMAL(18,2) NOT NULL,
    Amount INT UNSIGNED NOT NULL,
    CategoryId INT NOT NULL,
    FOREIGN KEY (CategoryId) REFERENCES category(Id) ON DELETE CASCADE
);
