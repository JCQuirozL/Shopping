namespace Shopping.Enums
{
    /// <summary>
    /// Supporting document types that justify an inventory adjustment.
    /// A stock change should always be traceable to one of these.
    /// </summary>
    public enum InventoryDocumentType
    {
        FacturaCompra,
        PreFactura,
        OrdenDeCompra,
        TraspasoEntreAlmacenes,
    }
}
