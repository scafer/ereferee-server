CREATE TABLE "users" (
  "id" SERIAL PRIMARY KEY,
  "username" varchar,
  "password" varchar,
  "email" varchar
);

CREATE TABLE "games" (
  "id" SERIAL PRIMARY KEY,
  "timeStart" varchar,
  "timeEnd" varchar,
  "homeScore" int,
  "visitorScore" int,
  "homeColor" varchar,
  "visitorColor" varchar,
  "status" int,
  "creationDate" varchar,
  "userId" int,
  "homeTeamId" int,
  "visitorTeamId" int
);

CREATE TABLE "teams" (
  "id" SERIAL PRIMARY KEY,
  "name" varchar,
  "color" varchar
);

CREATE TABLE "athletes" (
  "id" SERIAL PRIMARY KEY,
  "name" varchar,
  "teamId" int
);

CREATE TABLE "events" (
  "id" SERIAL PRIMARY KEY,
  "description" varchar
);

CREATE TABLE "teamAthletes" (
  "id" SERIAL PRIMARY KEY,
  "teamId" int,
  "athleteId" int,
  "status" varchar,
  "role" varchar,
  "number" int,
  "dayStart" varchar
);

CREATE TABLE "userGames" (
  "id" SERIAL PRIMARY KEY,
  "userId" int,
  "gameId" int,
  "role" int
);

CREATE TABLE "gameEvents" (
  "id" SERIAL PRIMARY KEY,
  "userId" int,
  "gameId" int,
  "athleteId" int,
  "time" varchar,
  "eventDescription" varchar,
  "reg" varchar
);

CREATE TABLE "gameAthletes" (
  "id" SERIAL PRIMARY KEY,
  "gameId" int,
  "athleteId" int,
  "reg" varchar
);

ALTER TABLE "games" ADD FOREIGN KEY ("userId") REFERENCES "users" ("id");

ALTER TABLE "games" ADD FOREIGN KEY ("homeTeamId") REFERENCES "teams" ("id");

ALTER TABLE "games" ADD FOREIGN KEY ("visitorTeamId") REFERENCES "teams" ("id");

ALTER TABLE "athletes" ADD FOREIGN KEY ("teamId") REFERENCES "teams" ("id");

ALTER TABLE "teamAthletes" ADD FOREIGN KEY ("teamId") REFERENCES "teams" ("id");

ALTER TABLE "teamAthletes" ADD FOREIGN KEY ("athleteId") REFERENCES "athletes" ("id");

ALTER TABLE "userGames" ADD FOREIGN KEY ("userId") REFERENCES "users" ("id");

ALTER TABLE "userGames" ADD FOREIGN KEY ("gameId") REFERENCES "games" ("id");

ALTER TABLE "gameEvents" ADD FOREIGN KEY ("userId") REFERENCES "users" ("id");

ALTER TABLE "gameEvents" ADD FOREIGN KEY ("gameId") REFERENCES "games" ("id");

ALTER TABLE "gameEvents" ADD FOREIGN KEY ("athleteId") REFERENCES "athletes" ("id");

ALTER TABLE "gameAthletes" ADD FOREIGN KEY ("gameId") REFERENCES "games" ("id");

ALTER TABLE "gameAthletes" ADD FOREIGN KEY ("athleteId") REFERENCES "athletes" ("id");
