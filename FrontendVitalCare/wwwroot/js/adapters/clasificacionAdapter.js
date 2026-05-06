const ClasificacionAdapter = {

    baseUrl: "http://localhost:5080/api/clasificaciones",

    async obtenerTodos(filtro = "") {
        const res = await fetch(`${this.baseUrl}?filtro=${filtro}`);
        return await this._procesarRespuesta(res);
    },

    async obtenerPorId(id) {
        const res = await fetch(`${this.baseUrl}/${id}`);
        return await this._procesarRespuesta(res);
    },

    async crear(data) {
        const res = await fetch(this.baseUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        });

        return await this._procesarRespuesta(res);
    },

    async actualizar(id, data) {
        const res = await fetch(`${this.baseUrl}/${id}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        });

        return await this._procesarRespuesta(res);
    },

    async eliminar(id) {
        const res = await fetch(`${this.baseUrl}/${id}?idUsuario=1`, {
            method: "DELETE"
        });

        return await this._procesarRespuesta(res);
    },

    // 🔥 método interno (clave del adapter)
    async _procesarRespuesta(res) {
        const data = await res.json().catch(() => null);

        return {
            ok: res.ok,
            data: data,
            status: res.status
        };
    }
};