using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;




/*
============================================================
* SISTEMA     : Teste 4intelligence
* PROGRAMADORA: Sofia Vasconcellos
* CRIACAO     : 25/04/2022
* DESCRICAO   : Programa que o usuário salva uma lista de vídeos preferidos.
============================================================
*/


namespace Teste4I
{

    internal class Search
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Pesquisando Video no YouTube");
            Console.WriteLine("========================");
            Console.WriteLine("Insira Sua Busca:");


            try
            {
                new Search().Run().Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }

            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }

        private async Task Run()
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                //Adicione abaixo sua API KEY
                ApiKey = "REPLACE_ME",
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = Console.ReadLine();
            searchListRequest.MaxResults = 5;

            // Chame o método search.list para recuperar os resultados que correspondem ao termo de consulta especificado.
            var searchListResponse = await searchListRequest.ExecuteAsync();

            List<string> videos = new List<string>();
            List<string> channels = new List<string>();
            List<string> playlists = new List<string>();

            // Adiciona cada resultado à lista apropriada e exibe as listas de
            // vídeos, canais e listas de reprodução correspondentes.
            foreach (var searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        videos.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.VideoId));
                        break;

                    case "youtube#channel":
                        channels.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.ChannelId));
                        break;

                    case "youtube#playlist":
                        playlists.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.PlaylistId));
                        break;
                }
            }

            Console.WriteLine(String.Format("Videos:\n{0}\n", string.Join("\n", videos)));
            Console.WriteLine(String.Format("Channels:\n{0}\n", string.Join("\n", channels)));
            Console.WriteLine(String.Format("Playlists:\n{0}\n", string.Join("\n", playlists)));
        }
    }
}