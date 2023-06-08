// Query Employee

SELECT E.id, E.name, C.id as contract_id, C.salary, C.start_date, C.end_date, COUNT(I.*) as sale_count , SUM(I.total_cost) as total_revenue
FROM employee E
JOIN invoice I on I.staff_id = E.id
LEFT OUTER JOIN contract C on C.staff_id = E.id
WHERE I.purchase_time >= DATE '2023-06-01'
  AND I.purchase_time < DATE '2023-06-01' + INTERVAL '1 MONTH' 
GROUP BY E.id, C.id

// Report Customer