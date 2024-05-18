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
    rows integer NOT NULL,
    columns integer NOT NULL,
    "Name" character varying(50) NOT NULL,
    description character varying(800),
    created_date timestamp without time zone NOT NULL,
    modified_date timestamp without time zone NOT NULL
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
-- Name: robot_command; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.robot_command (
    id integer NOT NULL,
    "Name" character varying(50) NOT NULL,
    description character varying(800),
    is_move_command boolean NOT NULL,
    created_date timestamp without time zone NOT NULL,
    modified_date timestamp without time zone NOT NULL
);


ALTER TABLE public.robot_command OWNER TO postgres;

--
-- Name: robot_command_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.robot_command ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.robot_command_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Data for Name: map; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.map (id, rows, columns, "Name", description, created_date, modified_date) FROM stdin;
1	5	5	5x5 map	\N	2024-03-30 00:00:00	2024-03-30 00:00:00
\.


--
-- Data for Name: robot_command; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.robot_command (id, "Name", description, is_move_command, created_date, modified_date) FROM stdin;
1	LEFT	\N	t	2024-03-30 00:00:00	2024-03-30 00:00:00
2	RIGHT	\N	t	2024-03-30 00:00:00	2024-03-30 00:00:00
3	MOVE	\N	t	2024-03-30 00:00:00	2024-03-30 00:00:00
4	SHOOT	\N	f	2024-03-30 00:00:00	2024-03-30 00:00:00
\.


--
-- Name: map_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.map_id_seq', 1, true);


--
-- Name: robot_command_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.robot_command_id_seq', 4, true);


--
-- Name: robot_command pk_robot_command; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.robot_command
    ADD CONSTRAINT pk_robot_command PRIMARY KEY (id);


--
-- PostgreSQL database dump complete
--

