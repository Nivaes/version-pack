# Version Pack

GitHub Action para generar versiones semánticas a partir del nombre de una rama o de una referencia de versión. La acción expone la versión en varios formatos y, cuando genera una versión nueva, crea y publica el tag correspondiente.

## Características

- Calcula automáticamente el siguiente número de versión usando los tags existentes en `origin`.
- Detecta el canal de publicación a partir del prefijo de la rama.
- Permite forzar el identificador de prerelease (`alpha`, `beta` o `rc`).
- Puede convertir una referencia existente, por ejemplo `release/1.2.3`, en los outputs de la acción.
- Publica el tag generado en el repositorio cuando no se proporciona el input `version`.

## Uso básico

```yaml
name: Version

on:
  push:
    branches:
      - 'dev/**'
      - 'alpha/**'
      - 'beta/**'
      - 'preview/**'
      - 'rc/**'
      - 'release/**'

permissions:
  contents: write

jobs:
  version:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v5
        with:
          fetch-depth: 0

      - name: Generar versión
        id: version
        uses: nivaes/version-pack@v2

      - name: Mostrar versión
        run: |
          echo "Número: ${{ steps.version.outputs.version-number }}"
          echo "Paquete: ${{ steps.version.outputs.version-pack }}"
          echo "Release: ${{ steps.version.outputs.version-release }}"
```

## Inputs

| Input | Requerido | Descripción |
| --- | --- | --- |
| `prerelease` | No | Fuerza el canal de prerelease. Valores reconocidos: `alpha`, `beta` y `rc`. |
| `version` | No | Referencia de versión existente con formato `<canal>/<versión>`, por ejemplo `release/1.2.3`. Si se proporciona, no se genera un nuevo tag. |

Si no se indica `version`, la acción obtiene la rama actual y genera una versión nueva. La parte final de la rama debe ser una versión mayor, por ejemplo `1.2` en `beta/1.2`.

## Outputs

| Output | Ejemplo | Descripción |
| --- | --- | --- |
| `version-number` | `1.2.3` | Número de versión semántica. |
| `version-pack` | `1.2-beta.3` | Versión con el identificador de prerelease, cuando corresponde. |
| `version-release` | `beta/1.2.3` | Referencia de release utilizada o generada. |

## Convenciones de ramas

Al generar una versión nueva, el prefijo de la rama determina el canal:

| Rama | Resultado habitual |
| --- | --- |
| `dev/1.2` | `1.2-alpha.N` |
| `alpha/1.2` | `1.2-alpha.N` |
| `beta/1.2` | `1.2-beta.N` |
| `preview/1.2` | `1.2-preview.N` |
| `rc/1.2` o `candidate/1.2` | `1.2-rc.N` |
| `release/1.2` | `1.2-preview.N` |

`N` se calcula buscando los tags existentes que comienzan por `1.2.` y tomando el siguiente número disponible. Por ejemplo, si el último tag es `1.2.4`, la siguiente versión numérica será `1.2.5`.

El input `prerelease` tiene prioridad sobre la convención de la rama:

```yaml
- uses: nivaes/version-pack@v2
  id: version
  with:
    prerelease: rc
```

## Usar una versión existente

Para obtener los outputs a partir de una referencia ya definida, proporciona `version`:

```yaml
- uses: nivaes/version-pack@v2
  id: version
  with:
    version: release/1.2.3

- run: echo "${{ steps.version.outputs.version-pack }}"
```

En este modo, `version-release` conserva el valor recibido y la acción no crea ni publica un tag nuevo.

## Requisitos

- Un runner Linux con `git` y el SDK de .NET disponible.
- `actions/checkout` antes de esta acción.
- `fetch-depth: 0` para que los tags estén disponibles localmente cuando el workflow los necesite.
- Permiso `contents: write` si se genera una versión nueva, porque la acción crea un tag anotado y ejecuta `git push origin --tags`.

La acción instala `dotnet-script` automáticamente durante la ejecución.

## Desarrollo y pruebas locales

El comportamiento de la acción está implementado en los scripts C# de [`src/`](./src/). El workflow [`test.yaml`](./.github/workflows/test.yaml) ejecuta una prueba de generación nueva y otra con una versión proporcionada.

Para probar la acción en un workflow local del repositorio:

```yaml
- uses: actions/checkout@v5
- uses: ./
  id: version
```

## Licencia

Este proyecto se distribuye bajo la licencia incluida en [`LICENSE`](./LICENSE).
