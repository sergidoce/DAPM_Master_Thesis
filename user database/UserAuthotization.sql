PGDMP      #                |            UserAuthorization    16.2    16.2     �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            �           1262    368927    UserAuthorization    DATABASE     �   CREATE DATABASE "UserAuthorization" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Chinese (Simplified)_China.936';
 #   DROP DATABASE "UserAuthorization";
                postgres    false            �            1259    368928    sys_interface_auth    TABLE     �   CREATE TABLE public.sys_interface_auth (
    "Id" text NOT NULL,
    "InterfaceName" text,
    "CreateTime" timestamp without time zone,
    "Status" bigint,
    "InterfaceType" text
);
 &   DROP TABLE public.sys_interface_auth;
       public         heap    postgres    false            �           0    0    TABLE sys_interface_auth    COMMENT     A   COMMENT ON TABLE public.sys_interface_auth IS '接口授权表';
          public          postgres    false    215            �           0    0    COLUMN sys_interface_auth."Id"    COMMENT     :   COMMENT ON COLUMN public.sys_interface_auth."Id" IS 'id';
          public          postgres    false    215            �           0    0 )   COLUMN sys_interface_auth."InterfaceName"    COMMENT     P   COMMENT ON COLUMN public.sys_interface_auth."InterfaceName" IS 'InterfaceName';
          public          postgres    false    215            �           0    0 &   COLUMN sys_interface_auth."CreateTime"    COMMENT     J   COMMENT ON COLUMN public.sys_interface_auth."CreateTime" IS 'CreateTime';
          public          postgres    false    215            �           0    0 "   COLUMN sys_interface_auth."Status"    COMMENT     X   COMMENT ON COLUMN public.sys_interface_auth."Status" IS 'Status(1. Normal 0. Deleted)';
          public          postgres    false    215            �           0    0 )   COLUMN sys_interface_auth."InterfaceType"    COMMENT     P   COMMENT ON COLUMN public.sys_interface_auth."InterfaceType" IS 'InterfaceType';
          public          postgres    false    215            �            1259    368933    sys_userinfo    TABLE     �  CREATE TABLE public.sys_userinfo (
    "Id" text NOT NULL,
    "UserName" text,
    "Password" text,
    "Status" bigint,
    "CreateTime" timestamp without time zone,
    "LastLoginTime" timestamp without time zone,
    "GroupID" text,
    "OrganizationID" text,
    "RoleName" text,
    "permission_repositoryID" text,
    "permission_repository_createID" text,
    "permission_repository_readID" text,
    "permission_resource_readID" text,
    "permission_resource_downloadID" text
);
     DROP TABLE public.sys_userinfo;
       public         heap    postgres    false            �           0    0    COLUMN sys_userinfo."Id"    COMMENT     8   COMMENT ON COLUMN public.sys_userinfo."Id" IS 'userid';
          public          postgres    false    216            �           0    0    COLUMN sys_userinfo."UserName"    COMMENT     @   COMMENT ON COLUMN public.sys_userinfo."UserName" IS 'UserName';
          public          postgres    false    216            �           0    0    COLUMN sys_userinfo."Password"    COMMENT     @   COMMENT ON COLUMN public.sys_userinfo."Password" IS 'Password';
          public          postgres    false    216            �           0    0    COLUMN sys_userinfo."Status"    COMMENT     R   COMMENT ON COLUMN public.sys_userinfo."Status" IS 'Status(1. Normal 0. Deleted)';
          public          postgres    false    216            �           0    0     COLUMN sys_userinfo."CreateTime"    COMMENT     D   COMMENT ON COLUMN public.sys_userinfo."CreateTime" IS 'CreateTime';
          public          postgres    false    216            �           0    0 #   COLUMN sys_userinfo."LastLoginTime"    COMMENT     J   COMMENT ON COLUMN public.sys_userinfo."LastLoginTime" IS 'LastLoginTime';
          public          postgres    false    216            �           0    0    COLUMN sys_userinfo."GroupID"    COMMENT     >   COMMENT ON COLUMN public.sys_userinfo."GroupID" IS 'GroupID';
          public          postgres    false    216            �           0    0 $   COLUMN sys_userinfo."OrganizationID"    COMMENT     L   COMMENT ON COLUMN public.sys_userinfo."OrganizationID" IS 'OrganizationID';
          public          postgres    false    216            �           0    0    COLUMN sys_userinfo."RoleName"    COMMENT     ]   COMMENT ON COLUMN public.sys_userinfo."RoleName" IS 'RoleName(administrator, normal user )';
          public          postgres    false    216            �           0    0 -   COLUMN sys_userinfo."permission_repositoryID"    COMMENT     h   COMMENT ON COLUMN public.sys_userinfo."permission_repositoryID" IS 'access all resource in repository';
          public          postgres    false    216            �           0    0 4   COLUMN sys_userinfo."permission_repository_createID"    COMMENT     k   COMMENT ON COLUMN public.sys_userinfo."permission_repository_createID" IS 'create resource in repository';
          public          postgres    false    216            �           0    0 2   COLUMN sys_userinfo."permission_repository_readID"    COMMENT     k   COMMENT ON COLUMN public.sys_userinfo."permission_repository_readID" IS 'read all resource in repository';
          public          postgres    false    216                        0    0 0   COLUMN sys_userinfo."permission_resource_readID"    COMMENT     o   COMMENT ON COLUMN public.sys_userinfo."permission_resource_readID" IS 'read resource
execute/deploy pipeline';
          public          postgres    false    216                       0    0 4   COLUMN sys_userinfo."permission_resource_downloadID"    COMMENT     _   COMMENT ON COLUMN public.sys_userinfo."permission_resource_downloadID" IS 'download resource';
          public          postgres    false    216            �          0    368928    sys_interface_auth 
   TABLE DATA           l   COPY public.sys_interface_auth ("Id", "InterfaceName", "CreateTime", "Status", "InterfaceType") FROM stdin;
    public          postgres    false    215   �       �          0    368933    sys_userinfo 
   TABLE DATA           3  COPY public.sys_userinfo ("Id", "UserName", "Password", "Status", "CreateTime", "LastLoginTime", "GroupID", "OrganizationID", "RoleName", "permission_repositoryID", "permission_repository_createID", "permission_repository_readID", "permission_resource_readID", "permission_resource_downloadID") FROM stdin;
    public          postgres    false    216   �        T           2606    368939 (   sys_interface_auth sys_interface_auth_pk 
   CONSTRAINT     h   ALTER TABLE ONLY public.sys_interface_auth
    ADD CONSTRAINT sys_interface_auth_pk PRIMARY KEY ("Id");
 R   ALTER TABLE ONLY public.sys_interface_auth DROP CONSTRAINT sys_interface_auth_pk;
       public            postgres    false    215            V           2606    368941    sys_userinfo sys_userinfo1_pk 
   CONSTRAINT     ]   ALTER TABLE ONLY public.sys_userinfo
    ADD CONSTRAINT sys_userinfo1_pk PRIMARY KEY ("Id");
 G   ALTER TABLE ONLY public.sys_userinfo DROP CONSTRAINT sys_userinfo1_pk;
       public            postgres    false    216            �   �   x����J1�s�}5��Z��.���6�d3a���ӛd�ݥ��~�7�Q����)`$n7����[G$'voB��AG�p����
��64�O���WX't�߈��ڥ�XRf@c�v}:Kc���֛4d��d���4�0Ĳ�TJ<�E����?É���پ����u�t�,��6����z�Ҁ��N��;1/�Ģ���҅�]���v�Qs�S�hwf��BJ����]      �   t  x���1n�1�g�>@hP$EQ�!z�,�(��ݡ��EG8����d�Zd0-���`�F�Q��J�y��D%J� cwA�%*�j���G�+H��ι�G)��m�,g�G��`b椪�����?Ͽn�^��ga�i}�T�`A*JӘF9�tR�];W(��uB��K�aüX�t�7�$�їl����������k*k�w� �*H��� ��Ǟ��T��Cx��:��:sD�6J�m����ڡվ��-0�zS�Ug���j<B4s�ǲ�;���b��f��>o�љ��M�^�V�� �1*xz������Z�Zá��Mא�y�|)g<��4i�H�n.�j,�ξo?�	�Y:�^N��O��.     