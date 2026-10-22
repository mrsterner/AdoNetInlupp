CREATE TABLE Genre
(
    GenreId INT IDENTITY (1,1) PRIMARY KEY,
    Name    NVARCHAR(50) NOT NULL UNIQUE
);
CREATE TABLE Movie
(
    MovieId     INT IDENTITY (1,1) PRIMARY KEY,
    Title       NVARCHAR(200) NOT NULL,
    ReleaseYear INT           NOT NULL,
    GenreId     INT           NOT NULL,
    CONSTRAINT FK_Movie_Genre FOREIGN KEY (GenreId) REFERENCES Genre (GenreId)
);
INSERT INTO Genre (Name)
VALUES ('Sci-Fi'),
       ('Horror'),
       ('Action'),
       ('Comedy'),
       ('Drama');
INSERT INTO Movie (Title, ReleaseYear, GenreId)
VALUES ('Alien', 1979, (SELECT GenreId FROM Genre WHERE Name = 'Sci-Fi')),
       ('Blade Runner', 1982, (SELECT GenreId FROM Genre WHERE Name = 'Sci-Fi')),
       ('The Shining', 1980, (SELECT GenreId FROM Genre WHERE Name = 'Horror')),
       ('Die Hard', 1988, (SELECT GenreId FROM Genre WHERE Name = 'Action')),
       ('Ghostbusters', 1984, (SELECT GenreId FROM Genre WHERE Name = 'Comedy')),
       ('The Godfather', 1972, (SELECT GenreId FROM Genre WHERE Name = 'Drama'));