use northwind_dt;
-- Reporte 1
-- En un periodo indicado de 1 año, desglosar por trimestre, cuales fueron los productos
-- dentro del top 5, considerando las unidades vendidas.

select p.productName, md.Quantity, quarter(m.Date) trimestre
from Products p
join movementdetails md on p.productid = md.productid
join movements m on m.movementId = md.movementId
where year(m.Date) = '1996'
order by md.Quantity desc
limit 5; 

-- Reporte 2
-- Para un producto elegido y un periodo indicado, mostrar el desglose por mes del
-- comportamiento en ventas de dicho producto.
select p.productName, p.UnitPrice, md.Quantity, m.Date Mes
from Products p
join movementdetails md on p.productid = md.productid
join movements m on m.movementid = md.movementid
where month(m.Date) = '06'
order by md.Quantity desc
limit 10;

-- Reporte 3
-- Mostrar los 10 producto que hay más en los almacenes al igual de los que hay menos
-- de tres almacenes por lo menos

select p.productName, w.Description, wp.UnitsInStock
from products p
join warehouseproducts wp on p.productId = wp.productId
join warehouses w on w.warehouseID = wp.warehouseID
where w.warehouseID  between 1 and 3 and 4
order by wp.UnitsInStock desc
limit 5;
select p.productName, w.Description, wp.UnitsInStock
from products p
join warehouseproducts wp on p.productId = wp.productId
join warehouses w on w.warehouseID = wp.warehouseID
where wp.warehouseID  between 1 and 3 and 4
order by wp.UnitsInStock asc
limit 5;




