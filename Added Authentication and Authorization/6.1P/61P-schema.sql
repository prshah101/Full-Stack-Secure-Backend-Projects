--
-- PostgreSQL database dump
--

-- Dumped from database version 16.2
-- Dumped by pg_dump version 16.2

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: map; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.map (
    id integer NOT NULL,
    name character varying(50) NOT NULL,
    description character varying(800),
    columns integer NOT NULL,
    rows integer NOT NULL,
    createddate timestamp without time zone NOT NULL,
    modifieddate timestamp without time zone NOT NULL
);


ALTER TABLE public.map OWNER TO postgres;

--
-- Name: map_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.map ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.map_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: robotcommand; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.robotcommand (
    id integer NOT NULL,
    name character varying(50) NOT NULL,
    description character varying(800),
    ismovecommand boolean NOT NULL,
    createddate timestamp without time zone NOT NULL,
    modifieddate timestamp without time zone NOT NULL
);


ALTER TABLE public.robotcommand OWNER TO postgres;

--
-- Name: robotcommand_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.robotcommand ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.robotcommand_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: user; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."user" (
    id integer NOT NULL,
    email character varying(255) NOT NULL,
    firstname character varying(100) NOT NULL,
    lastname character varying(100) NOT NULL,
    passwordhash character varying(255) NOT NULL,
    description character varying(800),
    role character varying(50),
    createddate timestamp without time zone NOT NULL,
    modifieddate timestamp without time zone NOT NULL
);


ALTER TABLE public."user" OWNER TO postgres;

--
-- Name: user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."user" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.user_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Data for Name: map; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.map (id, name, description, columns, rows, createddate, modifieddate) FROM stdin;
5	Map 4	 Not Square	5	6	-infinity	-infinity
2	Map Test	\N	5	5	2024-04-01 23:16:54.70912	2024-04-25 22:11:25.094555
4	Map 4	 Not Square	5	5	-infinity	2024-04-25 22:11:42.579351
6	Map 4	\N	5	5	-infinity	2024-04-25 22:14:18.89393
14	Map 3	 Not Square	5	6	-infinity	-infinity
\.


--
-- Data for Name: robotcommand; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.robotcommand (id, name, description, ismovecommand, createddate, modifieddate) FROM stdin;
1	LEFT	\N	t	2022-07-30 00:00:00	2022-07-30 00:00:00
\.


--
-- Data for Name: user; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."user" (id, email, firstname, lastname, passwordhash, description, role, createddate, modifieddate) FROM stdin;
5	example@example5.com	John	Sharma	AQAAAAEAACcQAAAAEEQrxix7TzC3ylkc7ZMuRCXINGDlsMjJDkuEIA2fTeIBHaxzBnluTjt2vPSkyuNRXQ==	Some description2	User	2024-05-06 00:44:45.955217	2024-05-06 01:00:06.090909
6	example@example2.com	Mary	Ann	AQAAAAEAACcQAAAAEMytZeULBUnRDQNX+naTBa7EdcfCcSb9Jt8LqdLzSWEU3Q2w74pyN/wsoFRf6ofC5A==	Some description	Admin	2024-05-06 00:50:48.76073	2024-05-06 21:44:51.439182
\.


--
-- Name: map_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.map_id_seq', 15, true);


--
-- Name: robotcommand_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.robotcommand_id_seq', 5, true);


--
-- Name: user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.user_id_seq', 6, true);


--
-- Name: map pk_map; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.map
    ADD CONSTRAINT pk_map PRIMARY KEY (id);


--
-- Name: robotcommand pk_robotcommand; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.robotcommand
    ADD CONSTRAINT pk_robotcommand PRIMARY KEY (id);


--
-- PostgreSQL database dump complete
--

