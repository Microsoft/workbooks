//
// Author:
//   Aaron Bockover <abock@microsoft.com>
//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import * as React from 'react'
import * as ReactDOM from 'react-dom'
import { randomReactKey } from '../utils'
import { CodeCellResult } from '../evaluation'
import {
    ResultRenderer,
    ResultRendererRepresentation,
    getFirstRepresentationOfType
} from '../rendering'
import {
    ToStringRepresentationDataTypeName,
    ToStringRepresentationData
 } from './ToStringRenderer'

const RepresentationTypeName = 'Xamarin.Interactive.Representations.VerbatimHtml'

export default function VerbatimHtmlRendererFactory(result: CodeCellResult) {
    return getFirstRepresentationOfType(result, RepresentationTypeName)
        ? new VerbatimHtmlRenderer
        : null
}

class VerbatimHtmlRenderer implements ResultRenderer {
    getRepresentations(result: CodeCellResult) {
        // VerbatimHtml on the C# side intentionally does not expose the HTML data
        // to serialization since we will always send a ToStringRepresentation. This
        // avoids sending duplicate data across the wire.
        //
        // So, grab the ToStringRepresentation and render that as HTML instead.
        const rep = getFirstRepresentationOfType<ToStringRepresentationData>(
            result,
            ToStringRepresentationDataTypeName)

        if (rep)
            return [{
                key: randomReactKey(),
                displayName: 'HTML',
                component: VerbatimHtmlRepresentation,
                componentProps: {
                    value: rep.formats[0].value
                }
            }]
        return []
    }
}

class VerbatimHtmlRepresentation extends React.Component<
    { value: string },
    { width: number, height: number }> {
    constructor(props: any) {
        super(props)
        this.state = {
            width: 0,
            height: 0
        }
    }

    render() {
        return (
            <iframe
                className='renderer-VerbatimHtmlRepresentation-container'
                seamless={true}
                sandbox='allow-scripts allow-same-origin'
                srcDoc={this.props.value}
                style={{
                    border: 'none',
                    width: `${this.state.width}px`,
                    height: `${this.state.height}px`
                }}
                onLoad={e => {
                    const iframe = ReactDOM.findDOMNode(this) as any
                    const iframeDoc = iframe.contentWindow.document.documentElement
                    this.setState({
                        width: iframeDoc.scrollWidth,
                        height: iframeDoc.scrollHeight,
                    })
                }}/>
        )
    }
}