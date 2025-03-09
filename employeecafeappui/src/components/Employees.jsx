import React, { useState, useEffect } from 'react';
import { Button, AppBar, Toolbar, Typography, Dialog } from '@mui/material';
import axios from 'axios';
import { AgGridReact } from 'ag-grid-react';
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';
import { useParams, useNavigate } from 'react-router-dom';
import { Edit, Delete } from '@mui/icons-material'; 
import { IconButton } from '@mui/material';  
import AddEditEmployee from './AddEditEmployee';

const EmployeesPage = () => {
    const [employees, setEmployees] = useState([]);
    const [showEmployeeForm, setShowEmployeeForm] = useState(false);
    const [selectedEmployeeID, setSelectedEmployeeID] = useState(null);
    const [columnDefs] = useState([
        { headerName: "Employee ID", field: "employeeID", sortable: true },
        { headerName: "Name", field: "name", sortable: true },
        { headerName: "Email", field: "emailAddress", sortable: true },
        { headerName: "Phone Number", field: "phoneNumber", sortable: true },
        { headerName: "Gender", field: "gender", sortable: true },
        { headerName: "Days Worked", field: "daysWorked", sortable: true },
        { headerName: "Cafe Name", field: "cafe", sortable: true },
        {
            headerName: "Actions",
            field: "actions",
            cellRenderer: (params) => (
                <div>
                    <IconButton
                        color="primary"
                        onClick={() => handleEdit(params.data.employeeID)}
                    >
                        <Edit />
                    </IconButton>
                    <IconButton
                        color="secondary"
                        onClick={() => handleDelete(params.data.employeeID)}
                    >
                        <Delete />
                    </IconButton>
                </div>
            ),
        }
    ]);

    const { cafeId } = useParams();
    const navigate = useNavigate();

    const fetchEmployees = (cafeId) => {
        if (cafeId) {
            // Fetch employees for the specific cafe
            axios.get(`https://localhost:7032/api/Employee?cafeID=${cafeId}`)
                .then(response => {
                    setEmployees(response.data);
                })

                .catch(error => {
                    console.error("There was an error fetching the employees with cafeID:", error);
                });
        }
        else {
            axios.get(`https://localhost:7032/api/Employee`)
                .then(response => {
                    setEmployees(response.data);
                })

                .catch(error => {
                    console.error("There was an error fetching the employees:", error);
                });
        }
    };

    useEffect(() => {
        fetchEmployees(cafeId);
    }, [cafeId]);

    const handleDelete = (id) => {
        if (window.confirm('Are you sure you want to delete this employee?')) {
            axios.delete(`https://localhost:7032/api/Employee?id=${id}`)
                .then(() => {
                    fetchEmployees(cafeId);
                    /*setEmployees(employees.filter(employee => employee.id !== id));*/
                })
                .catch(error => console.log('Error deleting employee:', error));
        }
    };

    const handleEdit = (employeeID) => {
        // Redirect to Edit Employee form
        setSelectedEmployeeID(employeeID);
        setShowEmployeeForm(true);
        fetchEmployees();
    };
    const handleAddEmp = () => {
        setSelectedEmployeeID(null);
        setShowEmployeeForm(true);
        fetchEmployees();
    };
    const handleCloseEmpForm = () => {
        setShowEmployeeForm(false);
        setSelectedEmployeeID(null);
        fetchEmployees();
    };

    const handleOnSubmit = () => {
        console.log('Form submitted successfully');
        setShowEmployeeForm(false);
        setSelectedEmployeeID(null);
    };

    return (
        <div>
            <AppBar position="static" style={{ backgroundColor: '#303f9f' }}>
                <Toolbar>
                    <Typography variant="h6" style={{ flexGrow: 1 }}>
                        Employees
                    </Typography>
                    <Button variant="contained" color="secondary" style={{ padding: '10px' }} onClick={() => navigate('/')}>Home</Button>
                </Toolbar>
            </AppBar>
            <div style={{ padding: '20px' }}>
                <Button
                    variant="contained"
                    color="primary"
                    onClick={ handleAddEmp }
                    style={{ marginBottom: '20px' }}>
                    Add New Employee
                </Button>
                <Dialog open={showEmployeeForm} onClose={handleCloseEmpForm} maxWidth="sm" fullWidth>
                    <AddEditEmployee onClose={handleCloseEmpForm} employeeID={selectedEmployeeID} onSubmit={handleOnSubmit} />
                </Dialog>
                <div className="ag-theme-alpine" style={{ height: 500, width: '1000px' }}>
                    <AgGridReact
                        columnDefs={columnDefs}
                        rowData={employees}
                        pagination={true}
                    />
                </div>
            </div>
        </div>
    );
};

export default EmployeesPage;


