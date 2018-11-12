using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RPG.Audio
{
	public static class TerrainTextureFinder
	{
		/// <summary>
		/// Finds the Alpha Map cell corresponding to a position and returns that
		/// cell's texture mix
		/// </summary>
		/// <param name="pos">Position in world space, with x and z coordinates within the terrain's bounds</param>
		/// <returns>Alpha Map as float array showing alpha for texture at that index</returns>
		public static float[] GetTextureMix(this Terrain terrain, Vector3 pos)
		{
			TerrainData terrainData = terrain.terrainData;
			Vector3 terrainPos = terrain.transform.position;

			// Get alpha map data for cell the position falls in
			int mapX = (int)(((pos.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth);
			int mapZ = (int)(((pos.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight);
			float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);
			// Convert to 1D array
			float[] cellMix = new float[splatmapData.GetUpperBound(2) + 1];
			for (int n = 0; n < cellMix.Length; ++n)
			{
				cellMix[n] = splatmapData[0, 0, n];
			}
			return cellMix;
		}

		/// <summary>
		/// Finds the Alpha Map cell corresponding to a position and returns the index
		/// of the texture predominating in that cell
		/// </summary>
		/// <param name="pos">Position in world space, with x and z coordinates within the terrain's bounds</param>
		/// <returns>Index of texture with greatest Alpha</returns>
		public static int GetMainTexture(this Terrain terrain, Vector3 pos)
		{
			// Get alpha map for cell position falls in
			float[] mix = terrain.GetTextureMix(pos);
			// Find index with greatest alpha
			float max = 0;
			int maxIndex = 0;
			for (int n = 0; n < mix.Length; ++n)
			{
				if (mix[n] > max)
				{
					max = mix[n];
					maxIndex = n;
				}
			}
			return maxIndex;
		}
	}
}