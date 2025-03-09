import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { ClientSideRowModelModule } from 'ag-grid-community'; 
import { ModuleRegistry } from 'ag-grid-community';

// Register the required modules
ModuleRegistry.registerModules([ClientSideRowModelModule]);
import HomePage from './components/Home';
import CafeList from './components/Cafes';
import EmployeeList from './components/Employees';


function App() {
    return (
        <Router>
            <div>
                <Routes>
                    {/* Route for viewing cafes */}
                    <Route path="/cafes" element={<CafeList />} />

                    {/* Route for viewing employees */}
                    <Route path="/employees" element={<EmployeeList />} />

                    {/* Route for viewing employees for respective cafe */}
                    <Route path="/employees/:cafeId" element={<EmployeeList/>} />

                    {/* Default Route to HomePage if no match */}
                    <Route path="/" exact element={<HomePage />} />
                </Routes>
            </div>
        </Router>
    );
}

export default App;
