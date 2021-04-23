CREATE TABLE "users" (
  "id" SERIAL PRIMARY KEY,
  "username" varchar UNIQUE NOT NULL,
  "password" varchar NOT NULL,
  "email" varchar UNIQUE NOT NULL,
  "accessLevel" int DEFAULT 0,
  "notifications" int,
  "status" int,
  "imageId" int,
  "created" varchar,
  "updated" varchar
);

CREATE TABLE "athletes" (
  "id" SERIAL PRIMARY KEY,
  "key" varchar UNIQUE,
  "name" varchar,
  "fullname" varchar,
  "birthDate" varchar,
  "birthPlace" varchar,
  "citizenship" varchar,
  "height" float,
  "weight" float,
  "position" varchar,
  "positionKey" int,
  "agent" varchar,
  "currentInternational" varchar,
  "status" varchar,
  "clubId" int,
  "imageId" int,
  "created" varchar,
  "updated" varchar
);

CREATE TABLE "clubs" (
  "id" SERIAL PRIMARY KEY,
  "key" varchar UNIQUE,
  "name" varchar,
  "fullname" varchar,
  "country" varchar,
  "founded" varchar,
  "colors" varchar,
  "members" numeric,
  "stadium" varchar,
  "address" varchar,
  "homepage" varchar,
  "imageId" int,
  "created" varchar,
  "updated" varchar
);

CREATE TABLE "competitions" (
  "id" SERIAL PRIMARY KEY,
  "key" varchar UNIQUE,
  "name" varchar,
  "edition" varchar,
  "sportId" int,
  "imageId" int,
  "created" varchar,
  "updated" varchar
);

CREATE TABLE "competitionBoards" (
  "id" SERIAL PRIMARY KEY,
  "position" int,
  "played" int,
  "won" int,
  "drawn" int,
  "lost" int,
  "goalsFor" int,
  "goalsAgainst" int,
  "goalsDifference" int,
  "points" int,
  "clubId" int,
  "competitionId" int,
  "created" varchar,
  "updated" varchar
);

CREATE TABLE "events" (
  "id" SERIAL PRIMARY KEY,
  "key" varchar UNIQUE,
  "name" varchar,
  "description" varchar,
  "sportId" int,
  "imageId" int,
  "created" varchar,
  "updated" varchar
);

CREATE TABLE "gameEvents" (
  "id" SERIAL PRIMARY KEY,
  "key" varchar UNIQUE,
  "time" varchar,
  "gameTime" varchar,
  "eventDescription" varchar,
  "gameId" int,
  "eventId" int,
  "athleteId" int,
  "userId" int,
  "created" varchar,
  "updated" varchar
);

CREATE TABLE "gameAthletes" (
  "id" SERIAL PRIMARY KEY,
  "status" int,
  "gameId" int,
  "athleteId" int,
  "created" varchar,
  "updated" varchar
);

CREATE TABLE "games" (
  "id" SERIAL PRIMARY KEY,
  "timeStart" varchar,
  "timeEnd" varchar,
  "homeColor" varchar,
  "visitorColor" varchar,
  "homeScore" int,
  "visitorScore" int,
  "homePenaltyScore" int,
  "visitorPenaltyScore" int,
  "status" int,
  "type" varchar,
  "location" varchar,
  "homeId" int,
  "visitorId" int,
  "competitionId" int,
  "imageId" int,
  "userId" int,
  "created" varchar,
  "updated" varchar
);

CREATE TABLE "gameUser" (
  "id" SERIAL PRIMARY KEY,
  "userId" int,
  "gameId" int,
  "athleteId" int,
  "created" varchar,
  "updated" varchar
);

CREATE TABLE "sports" (
  "id" SERIAL PRIMARY KEY,
  "name" varchar,
  "imageId" int,
  "created" varchar,
  "updated" varchar
);

CREATE TABLE "images" (
  "id" SERIAL PRIMARY KEY,
  "image" varchar,
  "imageUrl" varchar,
  "created" varchar,
  "updated" varchar
);

ALTER TABLE "users" ADD FOREIGN KEY ("imageId") REFERENCES "images" ("id");

ALTER TABLE "athletes" ADD FOREIGN KEY ("clubId") REFERENCES "clubs" ("id");

ALTER TABLE "athletes" ADD FOREIGN KEY ("imageId") REFERENCES "images" ("id");

ALTER TABLE "clubs" ADD FOREIGN KEY ("imageId") REFERENCES "images" ("id");

ALTER TABLE "competitions" ADD FOREIGN KEY ("sportId") REFERENCES "sports" ("id");

ALTER TABLE "competitions" ADD FOREIGN KEY ("imageId") REFERENCES "images" ("id");

ALTER TABLE "competitionBoards" ADD FOREIGN KEY ("clubId") REFERENCES "clubs" ("id");

ALTER TABLE "competitionBoards" ADD FOREIGN KEY ("competitionId") REFERENCES "competitions" ("id");

ALTER TABLE "events" ADD FOREIGN KEY ("sportId") REFERENCES "sports" ("id");

ALTER TABLE "events" ADD FOREIGN KEY ("imageId") REFERENCES "images" ("id");

ALTER TABLE "gameEvents" ADD FOREIGN KEY ("gameId") REFERENCES "games" ("id");

ALTER TABLE "gameEvents" ADD FOREIGN KEY ("eventId") REFERENCES "events" ("id");

ALTER TABLE "gameEvents" ADD FOREIGN KEY ("athleteId") REFERENCES "athletes" ("id");

ALTER TABLE "gameEvents" ADD FOREIGN KEY ("userId") REFERENCES "users" ("id");

ALTER TABLE "gameAthletes" ADD FOREIGN KEY ("gameId") REFERENCES "games" ("id");

ALTER TABLE "gameAthletes" ADD FOREIGN KEY ("athleteId") REFERENCES "athletes" ("id");

ALTER TABLE "games" ADD FOREIGN KEY ("homeId") REFERENCES "clubs" ("id");

ALTER TABLE "games" ADD FOREIGN KEY ("visitorId") REFERENCES "clubs" ("id");

ALTER TABLE "games" ADD FOREIGN KEY ("competitionId") REFERENCES "competitions" ("id");

ALTER TABLE "games" ADD FOREIGN KEY ("imageId") REFERENCES "images" ("id");

ALTER TABLE "games" ADD FOREIGN KEY ("userId") REFERENCES "users" ("id");

ALTER TABLE "gameUser" ADD FOREIGN KEY ("userId") REFERENCES "users" ("id");

ALTER TABLE "gameUser" ADD FOREIGN KEY ("gameId") REFERENCES "games" ("id");

ALTER TABLE "gameUser" ADD FOREIGN KEY ("athleteId") REFERENCES "athletes" ("id");

ALTER TABLE "sports" ADD FOREIGN KEY ("imageId") REFERENCES "images" ("id");

COMMENT ON COLUMN "gameEvents"."key" IS 'timestamp$userId';
