//
// Author:
//   Aaron Bockover <abock@microsoft.com>
//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import * as React from 'react'

import { CodeCellResult } from './evaluation'

export const enum ResultRendererRepresentationOptions {
    None = 0,

    /**
    * The representation will always be provided the expanded render
    * targets and will not be collapsible at all.
    */
    ForceExpand = 1,

    /**
     * The representation is collapsible, but will be expanded by default.
     */
    ExpandedByDefault = 2,

    /**
     * The representation is collapsible and will be collapsed by default if
     * it is the only or initially selected renderer, and otherwise expanded
     * automatically when selected from the menu.
     */
    ExpandedFromMenu = 4,

    /**
     * The display name of the representation will be suppressed in the
     * representation button label and shown only in the button's menu,
     * but only if all other representations also have the hint.
     */
    SuppressDisplayNameHint = 8
}

export interface ResultRenderer {
    getRepresentations(result: CodeCellResult): ResultRendererRepresentation[]
}

export type ResultRendererFactory = (result: CodeCellResult) => ResultRenderer | null

export interface ResultRendererRepresentation {
    key: string
    displayName: string
    component: any
    componentProps?: {}
    order?: number
    options?: ResultRendererRepresentationOptions
}

export function getRepresentationsOfType<T = {}>(result: CodeCellResult, typeName: string): T[] {
    const reps = []
    if (result && result.resultRepresentations) {
        for (const representation of result.resultRepresentations) {
            if (representation && representation.$type === typeName)
                reps.push(representation)
        }
    }
    return reps
}

export function getFirstRepresentationOfType<T = {}>(result: CodeCellResult, typeName: string): T | null {
    const reps = getRepresentationsOfType<T>(result, typeName)
    return reps && reps.length > 0
        ? reps[0]
        : null
}