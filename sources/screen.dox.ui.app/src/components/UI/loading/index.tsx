import React from 'react';
import styled from 'styled-components';

const PaheHight = styled.div`
    display: flex;
    justify-content: center;
    align-items: center;
    flex: 1;
    height: 100vh;
`;

const Loading = (): React.ReactElement => {
    return (
        <PaheHight>
            <div>
                <img width="70px" height="80px" src="../assets/logo-loader-transparent.gif" alt="logo-loader-transparent"/>
            </div>
        </PaheHight>
    )
}

export default Loading;