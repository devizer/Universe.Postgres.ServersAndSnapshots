SHOW LC_COLLATE;
Select datname, datcollate, datctype, * FROM pg_database where datname = current_database();
SELECT * FROM pg_collation;
SELECT current_database();
