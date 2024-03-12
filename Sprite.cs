﻿using static SDL2.SDL;

namespace Dysgenesis
{
    public abstract class Sprite
    {
        public const int POSITION_TIR_NON_EXISTANTE = -1;

        public Vector3 position;
        public Vector3[] modele = new Vector3[0] { };
        public SDL_Color couleure;
        public float taille = 1.0f;
        public float pitch = 0.0f;
        public float roll = 0.0f;
        public int[] indexs_lignes_sauter = new int[0];
        public int[] indexs_de_tir = new int[2];
        public int timer;
        public bool afficher = true;

        public float[] RenderLineData(int line_index, Vector3[] modele)
        {
            if (line_index >= modele.Length)
                return new float[4];

            float[] positions_ligne = new float[4];
            float sinroll = MathF.Sin(roll);
            float cosroll = MathF.Cos(roll);

            float grandeure_ligne = taille * MathF.Pow(0.95f, position.z);

            if (line_index == modele.Length - 1)
            {
                return new float[2]
                {
                    grandeure_ligne * (cosroll * -(modele[line_index].x) - sinroll * -(modele[line_index].y)) + position.x,
                    grandeure_ligne * (sinroll * -(modele[line_index].x) + cosroll * -(modele[line_index].y)) + position.y + modele[line_index].z * pitch,
                };
            }

            return new float[4]
            {
                grandeure_ligne * (cosroll * -(modele[line_index    ].x) - sinroll * -(modele[line_index    ].y)) + position.x,
                grandeure_ligne * (sinroll * -(modele[line_index    ].x) + cosroll * -(modele[line_index    ].y)) + position.y + modele[line_index    ].z * pitch,
                grandeure_ligne * (cosroll * -(modele[line_index + 1].x) - sinroll * -(modele[line_index + 1].y)) + position.x,
                grandeure_ligne * (sinroll * -(modele[line_index + 1].x) + cosroll * -(modele[line_index + 1].y)) + position.y + modele[line_index + 1].z * pitch
            };
        }
        public float[] RenderLineData(int line_index)
        {
            return RenderLineData(line_index, modele);
        }

        public virtual void RenderObject(Vector3[] modele)
        {
            if (!afficher)
                return;

            float[] positions_ligne;
            byte index_sauts = 0;

            SDL_SetRenderDrawColor(Program.render, couleure.r, couleure.g, couleure.b, couleure.a);

            for (int i = 0; i < modele.Length - 1; i++)
            {
                if (i == indexs_lignes_sauter[index_sauts] - 1)
                {
                    index_sauts++;
                    continue;
                }

                positions_ligne = RenderLineData(i, modele);

                SDL_RenderDrawLineF(Program.render, positions_ligne[0], positions_ligne[1], positions_ligne[2], positions_ligne[3]);
            }
        }
        public virtual void RenderObject()
        {
            RenderObject(modele);
        }

        public abstract bool Exist();
    }
}