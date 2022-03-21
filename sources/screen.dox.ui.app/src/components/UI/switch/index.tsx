import React from 'react';
import RegularSwitchComponent from './RegularSwitch';

export interface ISwitchComponent {
    type: 'regular';
    color?: 'regular' | 'regular-disabled';
    defaultValue?: boolean;
    switchHandler?: (v: boolean) => void; 
}

const SwitchComponent = (props: ISwitchComponent) => {
    switch (props.type) {
        case 'regular': return (
            <RegularSwitchComponent {...props} />
        )
        default: return null; 
    }
}

export default SwitchComponent;