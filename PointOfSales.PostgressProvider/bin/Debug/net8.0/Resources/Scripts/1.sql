CREATE SCHEMA IF NOT EXISTS "Security";
CREATE SCHEMA IF NOT EXISTS "Infrastructure";
CREATE TABLE IF NOT EXISTS "Infrastructure"."Company"
(
    "CompanyId" SMALLSERIAL PRIMARY KEY,
    "CompanyName" VARCHAR(255) NOT NULL,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
    "IsDeleted" BOOLEAN NOT NULL DEFAULT FALSE,
    "CreatedBy" INTEGER,
    "CreatedAt" TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    "ModifiedBy" INTEGER,
    "ModifiedAt" TIMESTAMP WITHOUT TIME ZONE,
    "LocationId" SMALLINT,
    "IsSyncStarted" BOOLEAN NOT NULL DEFAULT FALSE,
    "IsSyncEnd" BOOLEAN NOT NULL DEFAULT FALSE
);

CREATE TABLE IF NOT EXISTS "Infrastructure"."Location"
(
    "LocationId" SMALLSERIAL PRIMARY KEY,
    "LocationName" VARCHAR(100) NOT NULL,
    "LocationCode" VARCHAR(5) NOT NULL,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
    "IsDeleted" BOOLEAN NOT NULL DEFAULT FALSE,
    "CreatedBy" INTEGER,
    "CreatedAt" TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    "ModifiedBy" INTEGER,
    "ModifiedAt" TIMESTAMP WITHOUT TIME ZONE,
    "CompanyCode" INTEGER,
    "IsSyncStarted" BOOLEAN NOT NULL DEFAULT FALSE,
    "IsSyncEnd" BOOLEAN NOT NULL DEFAULT FALSE,
    FOREIGN KEY ("CompanyCode") REFERENCES "Infrastructure"."Company" ("CompanyId")
);


CREATE TABLE IF NOT EXISTS "Security"."Permission"
(
    "PermissionId"   SMALLSERIAL PRIMARY KEY,
    "PermissionName" TEXT UNIQUE                 NOT NULL,
    "PermissionCode" varchar(5) UNIQUE           NOT NULL,
    "IsActive"       BOOLEAN                     NOT NULL DEFAULT TRUE,
    "IsDeleted"      BOOLEAN                     NOT NULL DEFAULT FALSE,
    "CreatedBy"      INTEGER,
    "CreatedAt"      TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    "ModifiedBy"     INTEGER,
    "ModifiedAt"     TIMESTAMP WITHOUT TIME ZONE,
    "LocationId" SMALLINT,
    "IsSyncStarted" BOOLEAN NOT NULL DEFAULT FALSE,
    "IsSyncEnd" BOOLEAN NOT NULL DEFAULT FALSE,
    FOREIGN KEY ("LocationId") REFERENCES "Infrastructure"."Location" ("LocationId")
);

CREATE TABLE IF NOT EXISTS "Security"."User"
(
    "UserId"               SERIAL PRIMARY KEY,
    "UserName"             varchar(50) UNIQUE          NOT NULL,
    "Password"             varchar(255)                NOT NULL,
    "Salt"                 varchar(255)                NOT NULL,
    "ProfilePicture"       varchar(255),
    "PasswordExpiryDate"   TIMESTAMP WITHOUT TIME ZONE,
    "ShouldChangePassword" BOOLEAN                              DEFAULT TRUE,
    "IsActive"             BOOLEAN                     NOT NULL DEFAULT TRUE,
    "IsDeleted"            BOOLEAN                     NOT NULL DEFAULT FALSE,
    "CreatedBy"            INTEGER,
    "CreatedAt"            TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    "ModifiedBy"           INTEGER,
    "ModifiedAt"           TIMESTAMP WITHOUT TIME ZONE,
    "LocationId" SMALLINT,
    "IsSyncStarted" BOOLEAN NOT NULL DEFAULT FALSE,
    "IsSyncEnd" BOOLEAN NOT NULL DEFAULT FALSE,
    FOREIGN KEY ("LocationId") REFERENCES "Infrastructure"."Location" ("LocationId")

);
CREATE TABLE IF NOT EXISTS "Security"."UserPermission"
(
    "UserPermissionId" SERIAL PRIMARY KEY,
    "UserId"           INTEGER                     NOT NULL,
    "PermissionId"     SMALLINT                    NOT NULL,
    "EffectiveDate"    TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    "EndDate"          TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    "IsMfaRequired"    BOOLEAN                     NOT NULL DEFAULT FALSE,
    "IsActive"         BOOLEAN                     NOT NULL DEFAULT TRUE,
    "IsDeleted"        BOOLEAN                     NOT NULL DEFAULT FALSE,
    "CreatedBy"        INTEGER,
    "CreatedAt"        TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    "ModifiedBy"       INTEGER,
    "ModifiedAt"       TIMESTAMP WITHOUT TIME ZONE,
    "LocationId" SMALLINT,
    "IsSyncStarted" BOOLEAN NOT NULL DEFAULT FALSE,
    "IsSyncEnd" BOOLEAN NOT NULL DEFAULT FALSE,
    FOREIGN KEY ("LocationId") REFERENCES "Infrastructure"."Location" ("LocationId"),
    UNIQUE ("UserId", "PermissionId"),
    FOREIGN KEY ("UserId") REFERENCES "Security"."User" ("UserId"),
    FOREIGN KEY ("UserId") REFERENCES "Security"."Permission" ("PermissionId")
);
CREATE TABLE IF NOT EXISTS "Security"."Group"
(
    "GroupId"      SMALLSERIAL PRIMARY KEY,
    "Name"         varchar(40) UNIQUE          NOT NULL,
    "IsActive"     BOOLEAN                     NOT NULL DEFAULT TRUE,
    "IsDeleted"    BOOLEAN                     NOT NULL DEFAULT FALSE,
    "CreatedBy"    INTEGER,
    "CreatedAt"    TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    "ModifiedBy"   INTEGER,
    "ModifiedAt"   TIMESTAMP WITHOUT TIME ZONE,
    "LocationId" SMALLINT,
    "IsSyncStarted" BOOLEAN NOT NULL DEFAULT FALSE,
    "IsSyncEnd" BOOLEAN NOT NULL DEFAULT FALSE,
    FOREIGN KEY ("LocationId") REFERENCES "Infrastructure"."Location" ("LocationId")
);

CREATE TABLE IF NOT EXISTS "Security"."GroupPermission"
(
    "GroupPermissionId" SERIAL PRIMARY KEY,
    "GroupId"           SMALLINT                    NOT NULL,
    "PermissionId"      SMALLINT                    NOT NULL,
    "IsMfaRequired"     BOOLEAN                     NOT NULL DEFAULT FALSE,
    "IsActive"          BOOLEAN                     NOT NULL DEFAULT TRUE,
    "IsDeleted"         BOOLEAN                     NOT NULL DEFAULT FALSE,
    "CreatedBy"         INTEGER,
    "CreatedAt"         TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    "ModifiedBy"        INTEGER,
    "ModifiedAt"        TIMESTAMP WITHOUT TIME ZONE,
    "LocationId" SMALLINT,
    "IsSyncStarted" BOOLEAN NOT NULL DEFAULT FALSE,
    "IsSyncEnd" BOOLEAN NOT NULL DEFAULT FALSE,
    FOREIGN KEY ("LocationId") REFERENCES "Infrastructure"."Location" ("LocationId"),
    UNIQUE ("GroupId", "PermissionId"),
    FOREIGN KEY ("GroupId") REFERENCES "Security"."Group" ("GroupId"),
    FOREIGN KEY ("PermissionId") REFERENCES "Security"."Permission" ("PermissionId")
);

CREATE TABLE IF NOT EXISTS "Security"."UserGroup"
(
    "UserGroupId"  SERIAL PRIMARY KEY,
    "UserId"       INTEGER                     NOT NULL,
    "GroupId"      SMALLINT                    NOT NULL,
    "IsActive"     BOOLEAN                     NOT NULL DEFAULT TRUE,
    "IsDeleted"    BOOLEAN                     NOT NULL DEFAULT FALSE,
    "CreatedBy"    INTEGER,
    "CreatedAt"    TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    "ModifiedBy"   INTEGER,
    "ModifiedAt"   TIMESTAMP WITHOUT TIME ZONE,
    "LocationId" SMALLINT,
    "IsSyncStarted" BOOLEAN NOT NULL DEFAULT FALSE,
    "IsSyncEnd" BOOLEAN NOT NULL DEFAULT FALSE,
    FOREIGN KEY ("LocationId") REFERENCES "Infrastructure"."Location" ("LocationId"),
    UNIQUE ("UserId", "GroupId"),
    FOREIGN KEY ("UserId") REFERENCES "Security"."User" ("UserId"),
    FOREIGN KEY ("GroupId") REFERENCES "Security"."Group" ("GroupId")
);

CREATE TABLE IF NOT EXISTS "Infrastructure"."Device"
(
    "DeviceId" SMALLSERIAL PRIMARY KEY,
    "DeviceCode" VARCHAR(5) NOT NULL,
    "MachineCode" VARCHAR(255) NOT NULL,
    "LastActiveTime" TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    "IsActive" BOOLEAN NOT NULL DEFAULT FALSE,
    "IsDeleted" BOOLEAN NOT NULL DEFAULT FALSE,
    "CreatedBy" INTEGER,
    "CreatedAt" TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    "ModifiedBy" INTEGER,
    "ModifiedAt" TIMESTAMP WITHOUT TIME ZONE,
    "LocationId" SMALLINT,
    "IsSyncStarted" BOOLEAN NOT NULL DEFAULT FALSE,
    "IsSyncEnd" BOOLEAN NOT NULL DEFAULT FALSE,
    FOREIGN KEY ("LocationId") REFERENCES "Infrastructure"."Location" ("LocationId")
);

CREATE TABLE IF NOT EXISTS "Security"."ActivityLog"
(
    "ActivityLogId" UUID PRIMARY KEY,
    "PermissionId" SMALLINT NOT NULL,
    "UserId" INTEGER default null,
    "IsSuccess" BOOLEAN NOT NULL,
    "AccessedAt" TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    "OverrideUserId" INTEGER DEFAULT null,
    "Message" TEXT,
    "LocationId" SMALLINT NOT NULL,
    "DeviceId" SMALLINT NOT NULL,
    FOREIGN KEY ("PermissionId") REFERENCES "Security"."Permission" ("PermissionId"),
    FOREIGN KEY ("UserId") REFERENCES "Security"."User" ("UserId"),
    FOREIGN KEY ("OverrideUserId") REFERENCES "Security"."User" ("UserId"),
    FOREIGN KEY ("LocationId") REFERENCES "Infrastructure"."Location" ("LocationId"),
    FOREIGN KEY ("DeviceId") REFERENCES "Infrastructure"."Device" ("DeviceId")
);