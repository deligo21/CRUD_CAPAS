- Cambiar en propiedades de la solucion el proyecto de inicio a la aplicacion web
- En program.cs anadir lo siguiente

```
builder.Services.AddScoped<IGenericRepository<Contacto>, ContactoRepository>();
builder.Services.AddScoped<IContactoService, ContactoService>();
```

- En HomeController modificar de la siguiente manera
```
private readonly IContactoService _contactoService;

public HomeController(IContactoService contactoServ)
{
    this._contactoService = contactoServ;
}
```

- En Models, crear la carpeta ViewModels y luego el archivo VMContacto.cs
```
public class VMContacto
{
    public int IdContacto { get; set; }

    public string? Nombre { get; set; }

    public string? Telefono { get; set; }

    public string? FechaNacimiento { get; set; }

}
```

- Luego en HomeController insertar lo siguiente:
```
[HttpGet]
public async Task<IActionResult> Lista()
{
    IQueryable<Contacto> queryContactoSQL = await _contactoService.ObtenerTodos();
    List<VMContacto> lista = queryContactoSQL.Select(c => new VMContacto()
    {
        IdContacto = c.IdContacto,
        Nombre = c.Nombre,
        Telefono = c.Telefono,
        FechaNacimiento = c.FechaNacimiento.Value.ToString("dd/MM/yyyy")
    }).ToList();
    return StatusCode(StatusCodes.Status200OK, lista);
}

[HttpPost]
public async Task<IActionResult> Insertar([FromBody] VMContacto modelo)
{
    Contacto NuevoModelo = new Contacto()
    {
        Nombre = modelo.Nombre,
        Telefono = modelo.Telefono,
        FechaNacimiento = DateTime.ParseExact(modelo.FechaNacimiento, "dd/MM,yyyy", CultureInfo.CreateSpecificCulture("es-BO"))

    };

    bool respuesta = await _contactoService.Insertar(NuevoModelo);

    return StatusCode(StatusCodes.Status200OK, new {valor = respuesta});
}

[HttpPost]
public async Task<IActionResult> Actualizar([FromBody] VMContacto modelo)
{
    Contacto NuevoModelo = new Contacto()
    {
        IdContacto = modelo.IdContacto,
        Nombre = modelo.Nombre,
        Telefono = modelo.Telefono,
        FechaNacimiento = DateTime.ParseExact(modelo.FechaNacimiento, "dd/MM,yyyy", CultureInfo.CreateSpecificCulture("es-BO"))

    };

    bool respuesta = await _contactoService.Actualizar(NuevoModelo);

    return StatusCode(StatusCodes.Status200OK, new { valor = respuesta });
}

[HttpDelete]
public async Task<IActionResult> Eliminar(int id)
{

    bool respuesta = await _contactoService.Eliminar(id);

    return StatusCode(StatusCodes.Status200OK, new { valor = respuesta });
}
```

- Luego en el Index.html colocar lo siguiente:
```
@{
    ViewData["Title"] = "Home Page";
}


<div class="row">
    <div class="col-sm-12">

        <!--Inicio tarjeta-->
        <div class="card">
            <div class="card-header">Contactos</div>
            <div class="card-body">

                <button class="btn btn-success" id="btnNuevo">Nuevo Contacto</button>

                <hr />

                <table class="table table-striped" id="tbContacto">
                    <thead>
                        <tr>
                            <th>Nombre</th>
                            <th>Telefono</th>
                            <th>Fecha Nacimiento</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>

            </div>
        </div>
        <!--Fin tarjeta-->

    </div>
</div>

<!--Inicio Modal-->
<div class="modal" tabindex="-1">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Detalle Contacto</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                <input type="hidden" id="txtIdContacto" value="0" />
                <div class="mb-2">
                    <label>Nombre</label>
                    <input type="text" class="form-control" id="txtNombre" />
                </div>
                <div class="mb-2">
                    <label>Telefono</label>
                    <input type="text" class="form-control" id="txtTelefono" />
                </div>
                <div class="mb-2">
                    <label>Fecha Nacimiento</label>
                    <input type="text" class="form-control" id="txtFechaNacimiento" />
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-primary" id="btnGuardar">Guardar</button>
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
            </div>
        </div>
    </div>
</div>
<!--Fin Modal-->
@section Scripts{

<script>

    const Modelo_base = {
        idContacto : 0,
        nombre : "",
        telefono : "",
        fechaNacimiento : ""
    }

    $(document).ready(() =>{

        listaContactos();
    })

    function mostrarModal(modelo){

        $("#txtIdContacto").val(modelo.idContacto);
        $("#txtNombre").val(modelo.nombre)
        $("#txtTelefono").val(modelo.telefono)
        $("#txtFechaNacimiento").val(modelo.fechaNacimiento)

        $('.modal').modal('show');
    }

    $("#btnNuevo").click(() => {

        mostrarModal(Modelo_base);
    })

    $("#btnGuardar").click(() => {
        let NuevoModelo = Modelo_base;

        NuevoModelo["idContacto"] = $("#txtIdContacto").val();
        NuevoModelo["nombre"]  = $("#txtNombre").val();
        NuevoModelo["telefono"]  = $("#txtTelefono").val();
        NuevoModelo["fechaNacimiento"]  = $("#txtFechaNacimiento").val();

        if($("#txtIdContacto").val() == "0"){
            fetch("Home/Insertar",{
                method:"POST",
                headers: {
                    'Content-Type': 'application/json;charset=utf-8'
                },
                body: JSON.stringify( NuevoModelo)
            })
            .then((response) => {
                return response.ok ? response.json() : Promise.reject(response)
            })
            .then((dataJson) => {

                if(dataJson.valor){
                    alert("registrado");
                      $('.modal').modal('hide');
                     listaContactos();
                }
            })
        }else{
            fetch("Home/Actualizar",{
                    method:"PUT",
                    headers: {
                        'Content-Type': 'application/json;charset=utf-8'
                    },
                    body: JSON.stringify( NuevoModelo)
                })
                .then((response) => {
                    return response.ok ? response.json() : Promise.reject(response)
                })
                .then((dataJson) => {

                    if(dataJson.valor){
                        alert("editado");
                          $('.modal').modal('hide');
                         listaContactos();
                    }
                })

        }

    })

    function listaContactos(){

        fetch("Home/Lista")
        .then((response) => {
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then((dataJson) => {

            $("#tbContacto tbody").html("");

            dataJson.forEach((item) => {

                $("#tbContacto tbody").append(
                    $("<tr>").append(
                        $("<td>").text(item.nombre),
                        $("<td>").text(item.telefono),
                        $("<td>").text(item.fechaNacimiento),
                        $("<td>").append(
                            $("<button>").addClass("btn btn-primary btn-sm me-2 btn-editar").data("modelo",item).text("Editar"),
                            $("<button>").addClass("btn btn-danger btn-sm btn-eliminar").data("id",item.idContacto).text("Eliminar")
                        )
                    )
                )

            })


        })

    }

    $("#tbContacto tbody").on("click",".btn-editar",function(){
        let contacto = $(this).data("modelo")

        mostrarModal(contacto)
    })


      $("#tbContacto tbody").on("click",".btn-eliminar",function(){
        let idcontacto = $(this).data("id")

        let resultado = window.confirm("¿Desea eliminar el contacto?");


        if(resultado){


              fetch("Home/Eliminar?id=" + idcontacto,{
                    method:"DELETE"
                })
                .then((response) => {
                    return response.ok ? response.json() : Promise.reject(response)
                })
                .then((dataJson) => {

                    if(dataJson.valor){
                         listaContactos();
                    }
                })


        }


    })
</script>

}
```
