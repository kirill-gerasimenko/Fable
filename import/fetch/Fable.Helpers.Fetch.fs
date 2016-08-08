[<Fable.Core.Erase>]
module internal Fable.Helpers.Fetch

open System
open Fable.Import.Fetch
open Fable.Core
open Fable.Import
open Fable.Core.JsInterop

type [<StringEnum; RequireQualifiedAccess>] HttpMethod =
| [<CompiledName("GET")>] GET 
| [<CompiledName("HEAD")>] HEAD 
| [<CompiledName("POST")>] POST 
| [<CompiledName("PUT")>] PUT 
| [<CompiledName("DELETE")>] DELETE 
| [<CompiledName("TRACE")>] TRACE
| [<CompiledName("CONNECT")>] CONNECT

[<KeyValueList>]
type IRequestProperties =
    interface end

[<KeyValueList>]
type RequestProperties =
    | Method of HttpMethod
    | Headers of U2<HeaderInit, obj>
    | Body of BodyInit
    | Mode of RequestMode
    | Credentials of RequestCredentials
    | Cache of RequestCache
    interface IRequestProperties

/// Retrieves data from the specified resource.
let inline fetchAsyncWithInit (req: RequestInfo, init: RequestProperties list) : Async<Response> = 
    GlobalFetch.fetch(req, unbox init) |> Async.AwaitPromise

/// Retrieves data from the specified resource.
let inline fetchAsync (req: RequestInfo) : Async<Response> = 
    GlobalFetch.fetch(req) |> Async.AwaitPromise

/// Retrieves data from the specified resource, parses the json and returns the data as an object of type 'T.
let inline fetchAsWithInit<'T> (req: RequestInfo, init: RequestProperties list) : Async<'T> = async {
    let! fetched = GlobalFetch.fetch(req, unbox init) |> Async.AwaitPromise
    let! json = fetched.text() |> Async.AwaitPromise
    return ofJson<'T> json
}

/// Retrieves data from the specified resource, parses the json and returns the data as an object of type 'T. 
let inline fetchAs<'T> (req: RequestInfo) : Async<'T> = async {
    let! fetched = GlobalFetch.fetch(req) |> Async.AwaitPromise
    let! json = fetched.text() |> Async.AwaitPromise
    return ofJson<'T> json    
}
