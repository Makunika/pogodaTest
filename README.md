# pogodaTest
SQL Скрипт для развертывания
CREATE TABLE Towns
(
    TownId INT Primary KEY,
    TownName VARCHAR(100) NOT NULL
);

CREATE TABLE Weathers
(
    WeatherId INT Primary KEY,
    TownId INT NOT NULL,
    Degree INT NOT NULL,
    About VARCHAR(100) NOT NULL,
    WeatherDateTime DATETIME,
    CONSTRAINT tw_fk FOREIGN KEY (TownId) REFERENCES Towns(TownId)
);
