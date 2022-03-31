using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CrudSemORM.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace CrudSemORM.Controllers
{
    public class PessoaController : Controller
    {
        private readonly IConfiguration _configuration;

        public PessoaController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public IActionResult Index()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("Connection")))
            {
                sqlConnection.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter("PessoaViewAll", sqlConnection);
                dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                dataAdapter.Fill(dtbl);
            }
            return View(dtbl);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit([Bind("PessoaID,Nome,DataNascimento,Rg,Cpf,NomeMae,Profissao")] PessoaViewModel pessoaViewModel)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("Connection")))
                {
                    sqlConnection.Open();
                    SqlCommand cmd = new SqlCommand("PessoaAddOrEdit", sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("PessoaID", pessoaViewModel.PessoaID);
                    cmd.Parameters.AddWithValue("Nome", pessoaViewModel.Nome);
                    cmd.Parameters.AddWithValue("DataNascimento", pessoaViewModel.DataNascimento);
                    cmd.Parameters.AddWithValue("Rg", pessoaViewModel.Rg);
                    cmd.Parameters.AddWithValue("Cpf", pessoaViewModel.Cpf);
                    cmd.Parameters.AddWithValue("NomeMae", pessoaViewModel.NomeMae);
                    cmd.Parameters.AddWithValue("Profissao", pessoaViewModel.Profissao);
                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pessoaViewModel);
        }

        public IActionResult AddOrEdit(int? id)
        {
            PessoaViewModel pessoaViewModel = new PessoaViewModel();
            if (id > 0 )
            {
                pessoaViewModel = BuscarPessoaByID(id);
            }
            return View(pessoaViewModel);
        }

        // GET: Pessoa/Delete/5
        public IActionResult Delete(int? id)
        {
            PessoaViewModel pessoaViewModel = BuscarPessoaByID(id);
            return View(pessoaViewModel);
        }

        // POST: Pessoa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("Connection")))
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("PessoaDeleteByID", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("PessoaID", id);                
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction(nameof(Index));
        }
        [NonAction]
        public PessoaViewModel BuscarPessoaByID(int? id)
        {
            PessoaViewModel pessoaViewModel = new PessoaViewModel();
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("Connection")))
            {
                sqlConnection.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter("PessoaViewByID", sqlConnection);
                dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                dataAdapter.SelectCommand.Parameters.AddWithValue("PessoaID", id);
                dataAdapter.Fill(dtbl);
                if (dtbl.Rows.Count == 1)
                {
                    pessoaViewModel.PessoaID = Convert.ToInt32(dtbl.Rows[0]["PessoaID"].ToString());
                    pessoaViewModel.Nome = dtbl.Rows[0]["Nome"].ToString();
                    pessoaViewModel.DataNascimento = Convert.ToDateTime(dtbl.Rows[0]["DataNascimento"].ToString());
                    pessoaViewModel.Rg = dtbl.Rows[0]["Rg"].ToString();
                    pessoaViewModel.Cpf = dtbl.Rows[0]["Cpf"].ToString();
                    pessoaViewModel.NomeMae = dtbl.Rows[0]["NomeMae"].ToString();
                    pessoaViewModel.Profissao = dtbl.Rows[0]["Profissao"].ToString();
                }
                return pessoaViewModel;
            }
        }
    }
}
