SHOW LC_COLLATE;
Select datname, datcollate, datctype, * FROM pg_database where datname = current_database();
SELECT current_database();