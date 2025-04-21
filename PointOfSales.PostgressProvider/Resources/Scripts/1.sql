CREATE TABLE IF NOT EXISTS "Security"."Permission" (
                                            "PermissionId" SMALLSERIAL PRIMARY KEY,
                                            "PermissionName" TEXT NOT NULL,

                                            "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
                                            "IsDeleted" BOOLEAN NOT NULL DEFAULT FALSE,
                                            "CreatedBy" INTEGER,
                                            "CreatedAt" TIMESTAMP WITHOUT TIME ZONE NOT NULL,
                                            "ModifiedBy" INTEGER,
                                            "ModifiedAt" TIMESTAMP WITHOUT TIME ZONE,
                                            "CompanyCode" INTEGER,
                                            "LocationCode" INTEGER
);