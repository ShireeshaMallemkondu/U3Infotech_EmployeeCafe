import React, { useState, useEffect, useMemo } from 'react';
import { Button, TextField, AppBar, Toolbar, Typography, Dialog, IconButton } from '@mui/material';
import axios from 'axios';
import { AgGridReact } from 'ag-grid-react';
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';
import { useNavigate } from 'react-router-dom';
import { Edit,Delete } from '@mui/icons-material'; // Import Material UI Delete icon
import AddEditCafe from './AddEditCafe';


const CafesPage = () => {
    const navigate = useNavigate(); 
    const [cafes, setCafes] = useState([]);
    const [selectedCafeID, setSelectedCafeID] = useState(null);
    const [showCafeForm, setShowCafeForm] = useState(false);
    const [locationFilter, setLocationFilter] = useState('');
    const [error, setError] = useState(null);

    const fetchCafes = () => {
        axios.get('https://localhost:7032/api/Cafe')
            .then(response => {
                setCafes(response.data);
            })
            .catch(error => {
                console.error("There was an error fetching the cafes:", error);
                setError(error);
            });
    };

    useEffect(() => {
        fetchCafes();
    }, []);

    // Check cafes state after it's updated
    useEffect(() => {
        console.log('Cafes updated:', cafes); // Log updated cafes state
    }, [cafes]);

    const filteredCafes = useMemo(() => {
        return cafes.filter(cafe => cafe.location.toLowerCase().includes(locationFilter.toLowerCase()));
    }, [cafes, locationFilter]);

    // Filter handler
    const filterByLocation = (e) => {
        setLocationFilter(e.target.value);
    };

    const columnDefs = [
        { headerName: "Name", field: "name", sortable: true },
        { headerName: "Description", field: "description", sortable: true },
        {
            headerName: "Employees", field: "employees", sortable: true,
            cellRenderer: (params) => (
                <Button onClick={() => handleEmployees(params.data.id)}>View Employees</Button>)
        }, 
        { headerName: "Location", field: "location", sortable: true },
        {
            headerName: "Actions",
            field: "actions",
            cellRenderer: (params) => {
                return (
                    <div style={{ display: 'flex',marginBottom:'20px'  }}>
                    <IconButton
                        color="primary"
                        onClick={() => handleEdit(params.data.id)}
                        size ="small"
                    >
                        <Edit />
                        </IconButton>
                        <IconButton
                            color="secondary"
                            onClick={() => handleDelete(params.data.id)}
                        >
                            <Delete />
                        </IconButton>
                    </div>
                );
            },
          
        }
    ];

    const handleEmployees = (cafeId) => {
        navigate(`/employees/${cafeId}`);
    };
    const handleEdit = (cafeId) => {
        setSelectedCafeID(cafeId);
        setShowCafeForm(true);
    };

    const handleAddCafe = () => {
        setSelectedCafeID(null);
        setShowCafeForm(true);
    };

    const handleCloseCafeForm = () => {
        setShowCafeForm(false);
        setSelectedCafeID(null);
        fetchCafes();
    };
    const handleOnSubmit = () => {
        setShowCafeForm(false);
        setSelectedCafeID(null);
        fetchCafes();
    };
    const handleDelete = (id) => {
        if (window.confirm('Are you sure you want to delete this cafe?')) {
            axios.delete(`https://localhost:7032/api/Cafe?id=${id}`)
                .then(() => {
                    setCafes(cafes.filter(cafe => cafe.id !== id));
                })
                .catch(error => console.log(error));
        }
    };

    if (error) {
        return <div>{error}</div>;
    }

    return (
        <div>
            <AppBar position="static" style={{ backgroundColor: '#3f51b5' }}>
                <Toolbar>
                    <Typography variant="h6" style={{ flexGrow: 1 }}>
                        Cafes
                    </Typography>
                    <Button variant = "contained"color="secondary" onClick={() => navigate('/')}>Home</Button>
                </Toolbar>
            </AppBar>
            <div style={{ padding: '20px' }}>

                <TextField label="Filter by Location" value={locationFilter} onChange={filterByLocation} style={{ margin: '10px' }} />
                <Button
                    variant="contained"
                    color="primary"
                    onClick={handleAddCafe}
                    style={{ margin: '15px' }}
                >
                    Add New Cafe
                </Button>
                <Dialog open={showCafeForm} onClose={handleCloseCafeForm} maxWidth="sm" fullWidth>
                    <AddEditCafe onClose={handleCloseCafeForm} cafeId={selectedCafeID} onSubmit={handleOnSubmit} />
                </Dialog>
                {showCafeForm && <AddEditCafe onClose={handleCloseCafeForm} />}
                <div className="ag-theme-alpine" style={{ height: 500, width: '1000px' }}>
                    <AgGridReact
                        columnDefs={columnDefs}
                        rowData={filteredCafes}
                        pagination={true}
                    
                    />
                </div>
            </div>
        </div>
    );
};

export default CafesPage;