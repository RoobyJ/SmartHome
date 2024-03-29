PGDMP         *            	    {        	   SmartHome    15.3    15.3                0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false                       0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false                       0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false                       1262    139739 	   SmartHome    DATABASE     ~   CREATE DATABASE "SmartHome" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Polish_Poland.1250';
    DROP DATABASE "SmartHome";
                postgres    false            �            1259    139819    OutsideTemperature    TABLE     �   CREATE TABLE "Garages"."OutsideTemperature" (
    "Id" integer NOT NULL,
    "Date" timestamp with time zone NOT NULL,
    "Temperature" integer NOT NULL,
    "GarageId" integer NOT NULL
);
 +   DROP TABLE "Garages"."OutsideTemperature";
       Garages         heap    postgres    false            �            1259    139818    OutsideTemperature_Id_seq    SEQUENCE     �   CREATE SEQUENCE "Garages"."OutsideTemperature_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 5   DROP SEQUENCE "Garages"."OutsideTemperature_Id_seq";
       Garages          postgres    false    229                       0    0    OutsideTemperature_Id_seq    SEQUENCE OWNED BY     c   ALTER SEQUENCE "Garages"."OutsideTemperature_Id_seq" OWNED BY "Garages"."OutsideTemperature"."Id";
          Garages          postgres    false    228            �           2604    139822    OutsideTemperature Id    DEFAULT     �   ALTER TABLE ONLY "Garages"."OutsideTemperature" ALTER COLUMN "Id" SET DEFAULT nextval('"Garages"."OutsideTemperature_Id_seq"'::regclass);
 K   ALTER TABLE "Garages"."OutsideTemperature" ALTER COLUMN "Id" DROP DEFAULT;
       Garages          postgres    false    228    229    229                      0    139819    OutsideTemperature 
   TABLE DATA           Z   COPY "Garages"."OutsideTemperature" ("Id", "Date", "Temperature", "GarageId") FROM stdin;
    Garages          postgres    false    229   �                  0    0    OutsideTemperature_Id_seq    SEQUENCE SET     M   SELECT pg_catalog.setval('"Garages"."OutsideTemperature_Id_seq"', 1, false);
          Garages          postgres    false    228            �           2606    139824 *   OutsideTemperature OutsideTemperature_pkey 
   CONSTRAINT     q   ALTER TABLE ONLY "Garages"."OutsideTemperature"
    ADD CONSTRAINT "OutsideTemperature_pkey" PRIMARY KEY ("Id");
 [   ALTER TABLE ONLY "Garages"."OutsideTemperature" DROP CONSTRAINT "OutsideTemperature_pkey";
       Garages            postgres    false    229            �           2606    139825    OutsideTemperature GarageId    FK CONSTRAINT     �   ALTER TABLE ONLY "Garages"."OutsideTemperature"
    ADD CONSTRAINT "GarageId" FOREIGN KEY ("GarageId") REFERENCES "Garages"."Garage"("Id");
 L   ALTER TABLE ONLY "Garages"."OutsideTemperature" DROP CONSTRAINT "GarageId";
       Garages          postgres    false    229                  x������ � �     