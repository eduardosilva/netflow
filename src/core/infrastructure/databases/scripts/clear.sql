-- Active: 1694545135107@@localhost@5432@netflow-db

-- Check if the table exists and then drop it if it does
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'users') THEN
        DROP TABLE users;
    END IF;
    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'workflow_step_approvals') THEN
        DROP TABLE workflow_step_approvals;
    END IF;
    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'workflow_step_roles') THEN
        DROP TABLE workflow_step_roles;
    END IF;
    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'workflow_step_roles') THEN
        DROP TABLE roles;
    END IF;
    IF EXISTS (
            SELECT column_name
            FROM information_schema.columns
            WHERE table_name = 'workflow_instances'
            AND column_name = 'current_step_id'
        ) THEN
            ALTER TABLE workflow_instances DROP COLUMN current_step_id;
    END IF;
    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'workflow_step_instances') THEN
        DROP TABLE workflow_step_instances;
    END IF;
    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'workflow_instances') THEN
        DROP TABLE workflow_instances;
    END IF;
    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'workflow_steps') THEN
        DROP TABLE workflow_steps;
    END IF;
    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'workflows') THEN
        DROP TABLE workflows;
    END IF;
    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'roles') THEN
        DROP TABLE roles;
    END IF;
    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = '__EFMigrationsHistory') THEN
        DROP TABLE "__EFMigrationsHistory";
    END IF;
END $$;